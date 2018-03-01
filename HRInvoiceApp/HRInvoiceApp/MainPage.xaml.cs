using HRInvoiceApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HRInvoiceApp
{
    public partial class MainPage : ContentPage
    {
        SQLiteAsyncConnection db = App.Database.GetInstance();

        public MainPage()
        {
            InitializeComponent();
            db.CreateTableAsync<Users>().Wait();
            db.InsertAsync(new Users() { UserLastName = "Banaan", UserFirstName = "Bassie" });
            GetUsers(db);


        }

        async void GetUsers(SQLiteAsyncConnection db)
        {
            var a = await db.Table<Users>().ToListAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ObservableCollection<Users> UserList = new ObservableCollection<Users>();
            BanaanListview.ItemsSource = UserList;

        }
    }
}
