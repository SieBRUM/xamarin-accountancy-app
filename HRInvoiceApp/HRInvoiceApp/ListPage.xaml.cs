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
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            BanaanListview.ItemsSource = await db.Table<User>().ToListAsync();
        }
    }
}