using HRInvoiceApp.Helpers;
using HRInvoiceApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HRInvoiceApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddAssignment : ContentPage
	{
        Assignment assignment;
        SQLiteAsyncConnection db;
        List<Department> departments;
        List<Company> companies;
        Department selectedDepartment;
        Company selectedCompany;

        public AddAssignment()
        {
            InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            db = App.Database.GetInstance();
            assignment = new Assignment();
        }

        public AddAssignment(Department preselectedDepartment)
        {
            InitializeComponent();
            db = App.Database.GetInstance();
            assignment = new Assignment() { DepartmentId = preselectedDepartment.DepartmentId };
            selectedDepartment = preselectedDepartment;
        }

        public AddAssignment(int CostCenterNumber)
        {
            InitializeComponent();
            db = App.Database.GetInstance();
            Task.Run(async () =>
            {
                assignment = await db.Table<Assignment>().FirstOrDefaultAsync(x => x.CostCenterNumber == CostCenterNumber);

                if (assignment == null)
                {
                    await DisplayAlert("Alert", "Meegestuurde assignment staat niet in de database.", "Ok");
                    assignment = new Assignment();
                    await Navigation.PopToRootAsync();
                    return;
                }
                selectedDepartment = await db.Table<Department>().FirstOrDefaultAsync(x => x.DepartmentId == assignment.DepartmentId);

                addAssignmentClientEmail.Text = assignment.ClientEmailAddress;
                addAssignmentClientFirstName.Text = assignment.ClientFirstName;
                addAssignmentClientLastName.Text = assignment.ClientLastName;
                addAssignmentClientMobileNumber.Text = assignment.ClientMobileNumber;
                addAssignmentCostCenterNumer.Text = CostCenterNumber.ToString();
                addAssignmentHourSalary.Text = assignment.HourSalary.ToString();
            });
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            companies = await db.Table<Company>().ToListAsync();
            addAssignmentCompanyPicker.ItemsSource = companies;

            if (selectedDepartment != null)
            {
                addAssignmentCompanyPicker.SelectedIndex = companies.FindIndex(x => x.CompanyId == selectedDepartment.CompanyId);
                selectedCompany = companies.Where(x => x.CompanyId == selectedDepartment.CompanyId).FirstOrDefault();
                departments = await db.Table<Department>().Where(x => x.CompanyId == selectedCompany.CompanyId).ToListAsync();
                addAssignmentDepartmentPicker.ItemsSource = departments;
                addAssignmentDepartmentPicker.SelectedIndex = departments.FindIndex(x => x.DepartmentId == selectedDepartment.DepartmentId);
            }
        }

        void SaveAssignment(object sender, EventArgs e)
        {
            foreach (var element in addAssignmentLayout.Children)
            {
                if (element is Entry entry)
                {
                    if (String.IsNullOrWhiteSpace(entry.Text) && entry.Placeholder.Contains("*"))
                    {
                        DisplayAlert("Alert", "Graag alle velden met een * invullen", "Ok");
                        return;
                    }
                }
                if (element is Picker picker)
                {
                    if (picker.SelectedItem == null)
                    {
                        DisplayAlert("Alert", "Graag een afdeling selecteren", "Ok");
                        return;
                    }
                }
            }

            if (!int.TryParse(addAssignmentCostCenterNumer.Text, out int CostCenterNumber))
            {
                DisplayAlert("Alert", "Graag een correct kostenplaatsnummer invoeren", "Ok");
                return;
            }

            // Ref: http://regexlib.com/REDetails.aspx?regexp_id=2243
            if (!Regex.IsMatch(addAssignmentHourSalary.Text, @"^([\u20AC]?[1-9]\d*\.\d{3}(?:,\d{2})?|[\u20AC]?[1-9]\d*(?:,\d{2})?|[\u20AC]?[1-9]\d*)$"))
            {
                DisplayAlert("Alert", "Graag een correct salaris invoeren", "Ok");
                return;
            }

            if(!InputValidationHelper.IsValidEmail(addAssignmentClientEmail.Text))
            {
                DisplayAlert("Alert", "Graag een correct email adres invoeren", "Ok");
                return;
            }

            if(Regex.IsMatch(addAssignmentClientMobileNumber.Text, @"[a-zA-Z]"))
            {
                DisplayAlert("Alert", "Graag een correct telefoonnnummer invoeren", "Ok");
                return;
            }

            string salary = addAssignmentHourSalary.Text.Replace(" ", "").Replace("€", "");

            if(!float.TryParse(salary, out float floatSalary))
            {
                DisplayAlert("Alert", "Graag een correct salaris invoeren", "Ok");
                return;
            }

            assignment.ClientEmailAddress = addAssignmentClientEmail.Text;
            assignment.ClientFirstName = addAssignmentClientFirstName.Text;
            assignment.ClientLastName = addAssignmentClientLastName.Text;
            assignment.ClientMobileNumber = addAssignmentClientMobileNumber.Text;
            assignment.DepartmentId = selectedDepartment.DepartmentId;
            assignment.HourSalary = floatSalary;

            Task.Run(async () =>
            {
                if (assignment.CostCenterNumber == 0)
                {
                    Assignment tempAssignment = await db.Table<Assignment>().FirstOrDefaultAsync(x => x.CostCenterNumber == CostCenterNumber);
                    assignment.CostCenterNumber = CostCenterNumber;
                    if (tempAssignment != null)
                    {
                        bool doOverwrite = await DisplayAlert("Alert", "Er is al een opdracht met dit kostenplaatsnummer. Wilt u deze overschrijven?", "Ja", "Nee");
                        if (doOverwrite)
                        {
                            await db.InsertAsync(assignment);
                        }
                        else
                        {
                            return;
                        }
                    }
                    await db.InsertAsync(assignment);
                }
                else
                {
                    assignment.CostCenterNumber = CostCenterNumber;
                    await db.UpdateAsync(assignment);
                }

                bool openAddWorkday = await DisplayAlert("Succes", "Opdracht succesvol toegevoegd. Wilt u een werkdag toevoegen?", "Ja", "Nee");
                if(openAddWorkday)
                {
                    // do some navigating
                }
            });
        }

        private void OnSelectedDepartmentChanged(object sender, EventArgs e)
        {
            if (addAssignmentDepartmentPicker.SelectedIndex != -1)
            {
                selectedDepartment = departments[addAssignmentDepartmentPicker.SelectedIndex];
            }
        }

        private async void OnSelectedCompanyChanged(object sender, EventArgs e)
        {
            selectedCompany = companies[addAssignmentCompanyPicker.SelectedIndex];
            departments = await db.Table<Department>().Where(x => x.CompanyId == selectedCompany.CompanyId).ToListAsync();
            addAssignmentDepartmentPicker.ItemsSource = departments;
        }
    }
}