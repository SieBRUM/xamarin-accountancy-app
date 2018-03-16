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
    public partial class WeekOverview : ContentPage
    {
        SQLiteAsyncConnection db = App.Database.GetInstance();

        public WeekOverview()
        {
            InitializeComponent();
        }
    }
}
