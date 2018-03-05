using HRInvoiceApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HRInvoiceApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddCompany : ContentPage
	{
        SQLiteAsyncConnection db;
        Company company;
        Department department;
        KvK kvk;
        Province province;
        User user;

        public AddCompany ()
		{
			InitializeComponent ();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            db = App.Database.GetInstance();

            Task.Run(async () =>
            {
                company = await db.Table<Company>().FirstOrDefaultAsync();
                company = new Company();
            });
        }

        void saveCompany(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {

                company.CompanyName = companyName.Text;
                company.AddressAddition = AddressAddition.Text;
                company.City = City.Text;
                company.Zipcode = ZipCode.Text;

                await db.InsertOrReplaceAsync(company);
                await DisplayAlert("Succes", "Instellingen succesvol opgeslagen.", "OK");
            });
        }
            
	}
}