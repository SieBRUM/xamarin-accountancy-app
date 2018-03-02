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
    public partial class Assignment_Page : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public Assignment_Page()
        {
            InitializeComponent();
            ToolbarItems.Add(new ToolbarItem() { Icon = "Images/addCompany_icon.png", Command = new Command(gotoAssignment) });

            Items = new ObservableCollection<string>
            {
                "Item 1",
                "Item 2",
                "Item 3",
                "Item 4",
                "Item 5"
            };
			
			MyListView.ItemsSource = Items;
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            gotoDetails();   
        }

        async void gotoAssignment()
        {
            var page = new AddAssignment();

            await Navigation.PushModalAsync(page);
        }

        async void gotoDetails()
        {
            var page = new CompanyDetails();

            await Navigation.PushModalAsync(page);
        }
    }
}
