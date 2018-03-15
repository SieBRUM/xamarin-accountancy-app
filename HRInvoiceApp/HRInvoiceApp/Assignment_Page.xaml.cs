using HRInvoiceApp.Tables;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HRInvoiceApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Assignment_Page : ContentPage
    {
        SQLiteAsyncConnection db = App.Database.GetInstance();

        public Assignment_Page()
        {
            InitializeComponent();
            ToolbarItems.Add(new ToolbarItem() { Icon = "Images/addCompany_icon.png", Command = new Command(gotoAddCompany) });
            companyListview.ItemTapped += CompanyListview_ItemTapped;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            companyListview.ItemsSource = await db.Table<Company>().ToListAsync();
        }

        private void CompanyListview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var company = e.Item as Company;
            Navigation.PushModalAsync(new AddCompany(company.CompanyId));
        }

        async void gotoAddCompany()
        {
            var page = new AddCompany();

            await Navigation.PushModalAsync(page);
        }
    }
}
