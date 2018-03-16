using HRInvoiceApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
            ToolbarItems.Add(new ToolbarItem() { Icon = "Images/addCompany_icon.png", Command = new Command(gotoAddCompany)});
            ToolbarItems.Add(new ToolbarItem() { Icon = "Images/addWorkday_icon.png", Command = new Command(gotoAssignment) });
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");

            //list sorting on datetime
            var workdayList = await db.Table<Workday>().ToListAsync();

            workdayList.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
            workdayList.Reverse();
            WorkdayListView.ItemsSource = workdayList;
            WorkdayListView.ItemTapped += WorkdayListView_ItemTapped;
            //ObservableCollection<string> listOfMonths = new ObservableCollection<string>();
            //listOfMonths.Add("Januari");
            //listOfMonths.Add("Februari");
            //listOfMonths.Add("Maart");
            //listOfMonths.Add("April");
            //listOfMonths.Add("Mei");
            //listOfMonths.Add("Juni");
            //listOfMonths.Add("Juli");
            //listOfMonths.Add("Augustus");
            //listOfMonths.Add("September");
            //listOfMonths.Add("Oktober");
            //listOfMonths.Add("November");
            //listOfMonths.Add("December");
        }

        private void WorkdayListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //gets selected item from list
            var workday = e.Item as Workday;
            DisplayAlert("Details van opdracht", "Gemaakte uren: " + workday.WorkedHours + "\n" +
                "Gereden kilometers: " + workday.TraveledDistance + "\n" +
                "Notitie: " + workday.Note + "\n" +
                "Datum: " + workday.Date + "\n" + 
                "Kostenplaatsnummer: " + workday.CostCenterNumber,"Sluit");
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
        public static List<DateTime> GetDates(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                             .Select(day => new DateTime(year, month, day))
                             .Where(dt => dt.DayOfWeek != DayOfWeek.Sunday &&
                                          dt.DayOfWeek != DayOfWeek.Saturday)
                             .ToList();
        }

    }
}