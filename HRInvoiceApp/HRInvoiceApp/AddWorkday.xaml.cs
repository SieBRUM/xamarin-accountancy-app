using HRInvoiceApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HRInvoiceApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddWorkday : ContentPage
	{
        SQLiteAsyncConnection db;
        Workday workday;

        List<Assignment> assignments;
        List<Department> departments;
        List<Company> companies;

        Assignment selectedAssignment;
        Department selectedDepartment;
        Company selectedCompany;

		public AddWorkday()
		{
			InitializeComponent();
            db = App.Database.GetInstance();
            workday = new Workday();
		}

        public AddWorkday(Assignment preselectedAssignment)
        {
            InitializeComponent();
            db = App.Database.GetInstance();
            workday = new Workday();
            selectedAssignment = preselectedAssignment;
        }

        public AddWorkday(int workdayId)
        {
            InitializeComponent();
            db = App.Database.GetInstance();

            Task.Run(async () =>
            {
                workday = await db.Table<Workday>().Where(x => x.WorkdayId == workdayId).FirstOrDefaultAsync();

                if(workday == null)
                {
                    await DisplayAlert("Alert", "Meegestuurde werkdag staat niet in de database.", "Ok");
                    workday = new Workday();
                    await Navigation.PopToRootAsync();
                    return;
                }

                selectedAssignment = await db.Table<Assignment>().Where(x => x.CostCenterNumber == workday.CostCenterNumber).FirstOrDefaultAsync();

                addWorkdayNote.Text = workday.Note;
                addWorkdayKilometers.Text = workday.TraveledDistance.ToString();
                addWorkdayHours.Text = workday.WorkedHours.ToString();
                addWorkdayExtraCosts.Text = workday.ExtraExpenses;
                addWorkdayDate.Date = workday.Date;
            });
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            addWorkdayDate.MaximumDate = DateTime.Now;

            companies = await db.Table<Company>().ToListAsync();
            addWorkdayCompany.ItemsSource = companies;

            if(selectedAssignment != null)
            {
                assignments = await db.Table<Assignment>().Where(x => x.DepartmentId == selectedAssignment.DepartmentId).ToListAsync();
                addWorkdayAssignment.ItemsSource = assignments;
                addWorkdayAssignment.SelectedIndex = assignments.FindIndex(x => x.CostCenterNumber == selectedAssignment.CostCenterNumber);

                departments = await db.Table<Department>().Where(x => x.DepartmentId == selectedAssignment.DepartmentId).ToListAsync();
                selectedDepartment = departments.Where(x => x.DepartmentId == selectedAssignment.DepartmentId).First();
                addWorkdayDepartment.ItemsSource = departments;
                addWorkdayDepartment.SelectedIndex = departments.FindIndex(x => x.DepartmentId == selectedDepartment.DepartmentId);

                selectedCompany = companies.Where(x => x.CompanyId == selectedDepartment.CompanyId).First();
                addWorkdayCompany.SelectedIndex = companies.FindIndex(x => x.CompanyId == selectedCompany.CompanyId);
            }
        }

        async void SaveWorkday(object sender, EventArgs e)
        {
            foreach (var element in addWorkdayLayout.Children)
            {
                if (element is Entry entry)
                {
                    if (String.IsNullOrWhiteSpace(entry.Text) && entry.Placeholder.Contains("*"))
                    {
                        await DisplayAlert("Alert", "Graag alle velden met een * invullen", "Ok");
                        return;
                    }
                }
                if (element is Picker picker)
                {
                    if (picker.SelectedItem == null)
                    {
                        await DisplayAlert("Alert", "Graag alle velden met een * selecteren", "Ok");
                        return;
                    }
                }
            }

            // Input check >:(

            workday.CostCenterNumber = selectedAssignment.CostCenterNumber;
            workday.Date = addWorkdayDate.Date;
            workday.ExtraExpenses = addWorkdayExtraCosts.Text;
            workday.Note = addWorkdayNote.Text;
            workday.TraveledDistance = int.Parse(addWorkdayKilometers.Text);
            workday.WorkedHours = int.Parse(addWorkdayHours.Text);

            if(workday.WorkdayId == 0)
            {
                await db.InsertAsync(workday);
            }
            else
            {
                await db.UpdateAsync(workday);
            }
        }

        private async void OnSelectedCompanyChanged(object sender, EventArgs e)
        {
            if(addWorkdayCompany.SelectedIndex != -1)
            {
                selectedCompany = companies[addWorkdayCompany.SelectedIndex];
                departments = await db.Table<Department>().Where(x => x.CompanyId == selectedCompany.CompanyId).ToListAsync();
                if(departments.Count == 0)
                {
                    await DisplayAlert("Alert", "Dit bedrijf heeft nog geen afdelingen.", "Ok");
                }
                addWorkdayDepartment.ItemsSource = departments;
                addWorkdayAssignment.ItemsSource = null;
            }
        }

        private async void OnSelectedDepartmentChanged(object sender, EventArgs e)
        {
            if(addWorkdayDepartment.SelectedIndex != -1)
            {
                selectedDepartment = departments[addWorkdayDepartment.SelectedIndex];
                assignments = await db.Table<Assignment>().Where(x => x.DepartmentId == selectedDepartment.DepartmentId).ToListAsync();
                if(assignments.Count == 0)
                {
                    await DisplayAlert("Alert", "Deze afdeling heeft nog geen opdrachten.", "Ok");
                }
                addWorkdayAssignment.ItemsSource = assignments;
            }
        }

        private void OnSelectedAssigmentChanged(object sender, EventArgs e)
        {
            if(addWorkdayAssignment.SelectedIndex != -1)
            {
                selectedAssignment = assignments[addWorkdayAssignment.SelectedIndex];
            }
        }
    }
}