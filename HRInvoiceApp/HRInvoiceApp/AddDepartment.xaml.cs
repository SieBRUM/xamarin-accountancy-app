using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRInvoiceApp.Helpers;
using HRInvoiceApp.Tables;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HRInvoiceApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddDepartment : ContentPage
	{
        SQLiteAsyncConnection db;
        List<Company> companies;
        Company selectedCompany;
        Department department;

		public AddDepartment()
		{
			InitializeComponent();
            db = App.Database.GetInstance();
            department = new Department();
		}

        // For page open
        public AddDepartment(Company preselectedCompany)
        {
            InitializeComponent();
            db = App.Database.GetInstance();
            department = new Department() { CompanyId = preselectedCompany.CompanyId };
            selectedCompany = preselectedCompany;
        }

        // For edit department purposes
        public AddDepartment(int departmentId)
        {
            InitializeComponent();
            db = App.Database.GetInstance();
            Task.Run(async () =>
            {
                department = await db.Table<Department>().FirstOrDefaultAsync(x => x.DepartmentId == departmentId);

                // Wat doen we als app hier in komt hier? (zou niet mogelijk moeten zijn)
                if(department == null)
                {
                    await DisplayAlert("Alert", "Meegestuurde department staat niet in de database.", "Ok");
                    await Navigation.PopToRootAsync();
                    return;
                }
                selectedCompany = await db.Table<Company>().FirstOrDefaultAsync(x => x.CompanyId == department.CompanyId);
                addDepartmentBankNumber.Text = department.BankaccountNumber;
                addDepartmentCostCenterNumber.Text = department.CostCenterNumber.ToString();
                addDepartmentDepartmentName.Text = department.DepartmentName;
            });
        }

        // Override OnAppearing function so alert appears when Page is opened
        protected override void OnAppearing()
        {
            base.OnAppearing();

            Task.Run(async () =>
            {
                companies = await db.Table<Company>().ToListAsync();

                if(companies.Count() == 0)
                {
                    bool doNavigate = await DisplayAlert("Alert", "U moet eerst een bedrijf toevoegen voordat u een afdelding toe kan voegen. Wilt u naar 'Bedrijf toevoegen' gaan?", "Ja", "Nee");
                    if (doNavigate)
                    {
                        // Werkt nogsteeds niet :(
                        //await Navigation.PushModalAsync(new AddCompany());
                    }
                    return;
                }

                companyPicker.ItemsSource = companies;
                if(selectedCompany != null)
                {
                    companyPicker.SelectedIndex = companies.FindIndex(x => x.CompanyId == selectedCompany.CompanyId);
                }
            });
        }

        void SaveDepartment(object sender, EventArgs e)
        {
            // Check of alle velden ingevuld zijn
            foreach (var element in addDepartmentStack.Children)
            {
                if(element is Entry entry)
                {
                    if(String.IsNullOrWhiteSpace(entry.Text) && entry.Placeholder.Contains("*"))
                    {
                        DisplayAlert("Alert", "Graag alle velden met een * invullen", "Ok");
                        return;
                    }
                }
                if(element is Picker picker)
                {
                    if(picker.SelectedItem == null)
                    {
                        DisplayAlert("Alert", "Graag een bedrijf selecteren", "Ok");
                        return;
                    }
                }
            }

            if(!InputValidationHelper.ValidateIban(addDepartmentBankNumber.Text))
            {
                DisplayAlert("Alert", "Graag een correcte IBAN invullen", "Ok");
                return;
            }

            if(!int.TryParse(addDepartmentCostCenterNumber.Text, out int result))
            {
                DisplayAlert("Alert", "Graag een correcte Kostenplaatsnummer invullen", "Ok");
                return;
            }

            department.BankaccountNumber = addDepartmentBankNumber.Text;
            department.CompanyId = selectedCompany.CompanyId;
            department.DepartmentName = addDepartmentDepartmentName.Text;
            department.CostCenterNumber = int.Parse(addDepartmentCostCenterNumber.Text);

            Task.Run(async () =>
            {
                // If id is 0, it's a new entry...
                if(department.DepartmentId == 0)
                {
                    await db.InsertAsync(department);
                }
                else
                {
                   await db.UpdateAsync(department);
                }
                bool openAddAssignment = await DisplayAlert("Succes", "Afdeling succesvol toegevoegd. Wilt u een opdracht toevoegen?", "Ja", "Nee");
                if(openAddAssignment)
                {
                    // Open page...
                }
            });
        }

        private void OnSelectedCompanyChanged(object sender, EventArgs e)
        {
            selectedCompany = companies[companyPicker.SelectedIndex];
        }
    }
}