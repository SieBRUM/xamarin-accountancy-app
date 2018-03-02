using HRInvoiceApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HRInvoiceApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListPage : ContentPage
	{
        SQLiteAsyncConnection db = App.Database.GetInstance();
        public ListPage ()
		{
			InitializeComponent ();
            ToolbarItems.Add(new ToolbarItem() { Icon = "Images/addCompany_icon.png", Command = new Command(gotoAssignment)});
            ToolbarItems.Add(new ToolbarItem() { Icon = "Images/addWorkday_icon.png", Command = new Command(gotoAddCompany)});
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            BanaanListview.ItemsSource = await db.Table<User>().ToListAsync();
        }
        async void gotoAssignment()
        {
            var page = new AddAssignment();

            await Navigation.PushModalAsync(page);
        }
        async void gotoAddCompany()
        {
            var page = new AddCompany();

            await Navigation.PushModalAsync(page);
        }

    }
}