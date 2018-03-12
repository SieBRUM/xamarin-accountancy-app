using HRInvoiceApp.Database;
using HRInvoiceApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace HRInvoiceApp
{
	public partial class App : Application
	{
        static AccountancyDatabase database; 

		public App ()
		{
			InitializeComponent();

			MainPage = new MainPage();
		}

        public static AccountancyDatabase Database
        {
            get
            {
                if(database == null)
                {
                    database = new AccountancyDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("accountancydb.db"));
                }

                return database;
            }
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
