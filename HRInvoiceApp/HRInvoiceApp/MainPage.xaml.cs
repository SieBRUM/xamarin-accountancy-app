﻿using SQLite;
using Xamarin.Forms;

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
            // REMOVE WHEN MERGING
            // SHOULD BE SETTINGSPAGE (but for fast debugging changed location)
            //Children.Add(new NavigationPage(new SettingsPage { }) { Title = "Instellingen" });
            Children.Add(new NavigationPage(new AddDepartment { }) { Title = "Afdeling toevoegen" });
        }
    }
}
