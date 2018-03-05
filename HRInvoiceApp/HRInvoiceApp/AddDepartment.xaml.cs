using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		public AddDepartment()
		{
			InitializeComponent();
            db = App.Database.GetInstance();
		}

        // For page open
        public AddDepartment(Company preselectedCompany)
        {

        }

        // For edit department purposes
        public AddDepartment(Department department)
        {

        }

        // Override OnAppearing function so alert appeares when Page is opened
        protected override void OnAppearing()
        {
            Task.Run(async () =>
            {
                companies = await db.Table<Company>().ToListAsync();

                if (companies.Count() == 0)
                {
                    bool doNavigate = await DisplayAlert("Alert", "U moet eerst een bedrijf toevoegen voordat u een afdelding toe kan voegen. Wilt u naar 'Bedrijf toevoegen' gaan?", "Ja", "Nee");
                    if (doNavigate)
                    {
                        // Seems to crash eyyy
                        //await Navigation.PushModalAsync(new AddCompany());
                    }
                    return;
                }

                companyPicker.ItemsSource = companies;
            });
        }

        void SaveDepartment(object sender, EventArgs e)
        {
            // Check of alle velden ingevuld zijn
            foreach (var element in addDepartmentStack.Children)
            {
                if(element is Entry)
                {
                    Entry entry = (Entry)element;
                    if(String.IsNullOrWhiteSpace(entry.Text))
                    {
                        DisplayAlert("Alert", "Graag alle velden met een * invullen", "Ok");
                        return;
                    }
                }
                if(element is Picker)
                {
                    Picker picker = (Picker)element;
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

            // Still needs input checks!!
            Company selectedComapny = (Company)companyPicker.SelectedItem;

            Department department = new Department()
            {
                BankaccountNumber = addDepartmentBankNumber.Text,
                CompanyId = selectedComapny.CompanyId,
                DepartmentName = addDepartmentDepartmentName.Text,
                CostCenterNumber = int.Parse(addDepartmentCostCenterNumber.Text)
            };

            Task.Run(async () =>
            {
                // Ask open page to add assignment, if so, prefill form based on Id below..
                int departmentId = await db.InsertOrReplaceAsync(department);
                bool openAddAssignment = await DisplayAlert("Succes", "Afdeling succesvol toegevoegd. Wilt u een opdracht toevoegen?", "Ja", "Nee");
                if(openAddAssignment)
                {
                    // Start page, add non-defauled constructor.
                }
            });
        }
	}
}