using HRInvoiceApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLiteNetExtensions;

namespace HRInvoiceApp
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            SQLiteAsyncConnection db = App.Database.GetInstance();
            db.InsertAsync(new User() { BankaccountNumber = "REEEEE" });
            getUsers(db);
        }

        async void getUsers(SQLiteAsyncConnection db)
        {
            var a =  await db.Table<User>().ToListAsync();
        }
	}
}
