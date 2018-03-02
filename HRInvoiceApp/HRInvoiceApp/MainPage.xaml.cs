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

            SQLiteAsyncConnection db = App.Database.GetInstance();

            BarBackgroundColor = Color.FromRgb(63, 182, 202);

            Children.Add(new NavigationPage(new Assignment_Page {  }) {Title = "Opdrachtoverzicht" });
            Children.Add(new NavigationPage(new ListPage { }) {Title = "Maandoverzicht" });
            Children.Add(new NavigationPage(new SettingsPage { }) {Title = "Instellingen" });

        }
	}
}
