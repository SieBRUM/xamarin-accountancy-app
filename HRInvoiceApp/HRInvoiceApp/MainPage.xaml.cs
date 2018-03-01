using HRInvoiceApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLiteNetExtensions;
using Windows.Gaming.Input;

namespace HRInvoiceApp
{
	public partial class MainPage : TabbedPage
	{
		public MainPage()
		{
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this,false);
            SQLiteAsyncConnection db = App.Database.GetInstance();
        }
	}
}
