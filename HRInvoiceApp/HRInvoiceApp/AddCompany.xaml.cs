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
        List<Province> provinces;
        User user;
        
        public AddCompany()
		{
			InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            db = App.Database.GetInstance();

            Task.Run(async () =>
            {
                user = await db.Table<User>().FirstOrDefaultAsync();
                provinces = await db.Table<Province>().ToListAsync();
                if(provinces.Count == 0)
                {
                    // Error handling
                }

                if(user == null)
                {
                    await DisplayAlert("Alert", "Voeg eerst een gebruiker toe.", "OK");
                    var modalPage = new SettingsPage();
                    await Navigation.PushModalAsync(modalPage);
                    return;
                }
                company = new Company();
                kvk = new KvK();

                ProvincePicker.ItemsSource = provinces;

            });

        }

        void saveCompany(object sender, EventArgs e)
        {
            foreach (var view in addCompanyStackLayout.Children)
            {
                if (view is Entry)
                {
                    Entry entry = (Entry)view;
                    if (string.IsNullOrWhiteSpace(entry.Text))
                    {
                        DisplayAlert("Alert", "Graag alle velden met een * invullen.", "OK");
                        return;
                    }
                }
            }
            Task.Run(async () =>
            {
                kvk.KvKNumber = int.Parse(KvKNumber.Text);
                var kvkId = await db.InsertAsync(kvk);
                
                company.KvkId = kvkId;
                company.Province = ((Province)ProvincePicker.SelectedItem).ProvinceName;
                company.UserId = user.UserId;
                company.CompanyName = companyName.Text;
                company.Address = Address.Text;
                company.AddressAddition = AddressAddition.Text;
                company.City = City.Text;
                company.Zipcode = ZipCode.Text;

                await db.InsertOrReplaceAsync(company);
                await DisplayAlert("Succes", "Instellingen succesvol opgeslagen.", "OK");
            });
        }
            
	}
}