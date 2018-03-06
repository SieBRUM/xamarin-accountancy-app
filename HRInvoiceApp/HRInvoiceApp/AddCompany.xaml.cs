using HRInvoiceApp.Helpers;
using HRInvoiceApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                    await DisplayAlert("Alert", "Er zijn geen provincies beschikbaar", "OK");
                    return;
                }

                if(user == null)
                {
                    await DisplayAlert("Alert", "Voeg eerst een gebruiker toe.", "OK");
                    //var modalPage = new SettingsPage();
                    //await Navigation.PushModalAsync(modalPage);
                    return;
                }
                kvk = new KvK();
                company = new Company();
                ProvincePicker.ItemsSource = provinces;

            });

        }

        void saveCompany(object sender, EventArgs e)
        {
            foreach (var view in addCompanyStackLayout.Children)
            {
                if (view is Entry entry)
                {
                    if (string.IsNullOrWhiteSpace(entry.Text) && entry.Placeholder.Contains("*"))
                    {
                        DisplayAlert("Alert", "Graag alle velden met een * invullen.", "OK");
                        return;
                    }
                }
                else if(view is Picker picker)
                {
                    if(picker.SelectedItem == null)
                    {
                        DisplayAlert("Alert", "Graag een provincie invullen.", "OK");
                        return;
                    }
                }
            }
            if (KvKNumber.Text.Count() != 8 || !int.TryParse(KvKNumber.Text, out int result))
            {
                DisplayAlert("Alert", "Graag een correct KvK nummer invullen.", "OK");
                return;
            }

            if(!InputValidationHelper.IsZipCodeValid(ZipCode.Text))
            {
                DisplayAlert("Alert", "Graag een geldige postcode invullen.", "OK");
                return;
            }

            Task.Run(async () =>
            {
                kvk.KvKNumber = int.Parse(KvKNumber.Text);
                if(kvk.Id == 0)
                {
                    await db.InsertAsync(kvk);
                }
                else
                {
                    await db.UpdateAsync(kvk);
                }
                company.KvKId = kvk.Id;
                company.Province = ((Province)ProvincePicker.SelectedItem).ProvinceName;
                company.UserId = user.UserId;
                company.CompanyName = companyName.Text;
                company.Address = Address.Text;
                company.AddressAddition = AddressAddition.Text;
                company.City = City.Text;
                company.Zipcode = ZipCode.Text;
                if(company.CompanyId == 0)
                {
                    await db.InsertAsync(company);
                }
                else
                {
                    await db.UpdateAsync(company);
                }
                await DisplayAlert("Succes", "Instellingen succesvol opgeslagen.", "OK");
            });
        }
            
	}
}