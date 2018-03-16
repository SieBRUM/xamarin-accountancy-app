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
                if (provinces.Count == 0)
                {
                    await DisplayAlert("Alert", "Er zijn geen provincies beschikbaar", "OK");
                    return;
                }

                if (user == null)
                {
                    await DisplayAlert("Alert", "Voeg eerst een gebruiker toe.", "OK");
                    //var modalPage = new SettingsPage();
                    //await Navigation.PushModalAsync(modalPage);
                    return;
                }
                kvk = new KvK();
                company = new Company();
                addCompanyProvincePicker.ItemsSource = provinces;
            });
        }
        public AddCompany(int companyId)
        {
            InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            db = App.Database.GetInstance();
            Task.Run(async () =>
            {
                company = await db.Table<Company>().FirstOrDefaultAsync(x => x.CompanyId == companyId);
                kvk = await db.Table<KvK>().FirstOrDefaultAsync(x => x.Id == companyId);
                provinces = await db.Table<Province>().ToListAsync();
                // Wat doen we als app hier in komt hier? (zou niet mogelijk moeten zijn)
                if (company == null)
                {
                    await DisplayAlert("Alert", "Meegestuurde company staat niet in de database.", "Ok");
                    await Navigation.PopToRootAsync();
                    return;
                }
                addCompanyCompanyName.Text = company.CompanyName;
                addCompanyKvKNumber.Text = kvk.KvKNumber.ToString();
                addCompanyAddress.Text = company.Address;
                addCompanyAddressAddition.Text = company.AddressAddition;
                addCompanyCity.Text = company.City;
                addCompanyProvincePicker.ItemsSource = provinces;
                addCompanyProvincePicker.SelectedIndex = provinces.FindIndex(x => x.ProvinceName == company.Province);
                addCompanyZipCode.Text = company.Zipcode;
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
                else if (view is Picker picker)
                {
                    if (picker.SelectedItem == null)
                    {
                        DisplayAlert("Alert", "Graag een provincie invullen.", "OK");
                        return;
                    }
                }
            }
            if (addCompanyKvKNumber.Text.Count() != 8 || !int.TryParse(addCompanyKvKNumber.Text, out int result))
            {
                DisplayAlert("Alert", "Graag een correct KvK nummer invullen.", "OK");
                return;
            }

            if (!InputValidationHelper.IsZipCodeValid(addCompanyZipCode.Text))
            {
                DisplayAlert("Alert", "Graag een geldige postcode invullen.", "OK");
                return;
            }

            Task.Run(async () =>
            {
                kvk.KvKNumber = int.Parse(addCompanyKvKNumber.Text);
                if (kvk.Id == 0)
                {
                    await db.InsertAsync(kvk);
                }
                else
                {
                    await db.UpdateAsync(kvk);
                }
                company.KvKId = kvk.Id;
                company.Province = ((Province)addCompanyProvincePicker.SelectedItem).ProvinceName;
                company.CompanyName = addCompanyCompanyName.Text;
                company.Address = addCompanyAddress.Text;
                company.AddressAddition = addCompanyAddressAddition.Text;
                company.City = addCompanyCity.Text;
                company.Zipcode = addCompanyZipCode.Text;
                if (company.CompanyId == 0)
                {
                    await db.InsertAsync(company);
                }
                else
                {
                    await db.UpdateAsync(company);
                }
                await DisplayAlert("Succes", "Instellingen succesvol opgeslagen.", "OK");

                //navigatie back to listview... to be implemented
            });
        }
    }
}