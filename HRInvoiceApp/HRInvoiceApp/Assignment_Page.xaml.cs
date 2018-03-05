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
            ToolbarItems.Add(new ToolbarItem() { Icon = "Images/addCompany_icon.png", Command = new Command(gotoAssignment) });
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            companyListview.ItemsSource = await db.Table<Company>().ToListAsync();
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            gotoDetails();   
        }

        async void gotoAssignment()
        {
            var page = new AddCompany();

            await Navigation.PushModalAsync(page);
        }

        async void gotoDetails()
        {
            var page = new CompanyDetails();

            await Navigation.PushModalAsync(page);
        }
    }
}
