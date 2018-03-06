using HRInvoiceApp.Database;
using HRInvoiceApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using HRInvoiceApp.Helpers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HRInvoiceApp
{

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
        SQLiteAsyncConnection db;
        User user;
        KvK kvk;

        public SettingsPage()
		{
			InitializeComponent();
            db = App.Database.GetInstance();

            Task.Run(async () =>
            {
                user = await db.Table<User>().FirstOrDefaultAsync();
                if (user != null)
                {
                    kvk = await db.Table<KvK>().FirstOrDefaultAsync(x => x.Id == user.KvKId);
                }
                else
                {
                    user = new User();
                    kvk = new KvK();
                }

                firstName.BindingContext = user.UserFirstName;
                bankNumber.BindingContext = user.BankaccountNumber;
                lastName.BindingContext = user.UserLastName;
                website.BindingContext = user.Website;
                mobileNumber.BindingContext = user.PhoneNumber;
                email.BindingContext = user.EmailAddress;
                vatNumber.BindingContext = user.VATNumber;
                kvkNumber.BindingContext = kvk.KvKNumber;
            });
        }

        void onSaveClicked(object sender, EventArgs e)
        {
            foreach (var view in settingsStackLayout.Children)
            {
                if(view is Entry entry)
                {
                    if(entry.Placeholder.Contains("*") && string.IsNullOrWhiteSpace(entry.Text))
                    {
                        DisplayAlert("Alert", "Graag alle velden met een * invullen.", "OK");
                        return;
                    }
                }
            }

            if(kvkNumber.Text.Count() != 8 || !int.TryParse(kvkNumber.Text, out int result))
            {
                DisplayAlert("Alert", "Graag een correct KvK nummer invullen.", "OK");
                return;
            }
            if (!InputValidationHelper.IsValidEmail(email.Text))
            {
                DisplayAlert("Alert", "Graag een correct email adres nummer invullen.", "OK");
                return;
            }
            if(!InputValidationHelper.ValidateIban(bankNumber.Text))
            {
                DisplayAlert("Alert", "Graag een correct IBAN nummer invullen.", "OK");
                return;
            }
            if(!InputValidationHelper.ValidateVATNumber(vatNumber.Text))
            {
                DisplayAlert("Alert", "Graag een correct BTW nummer invullen.", "OK");
            }

            Task.Run(async () =>
            {
                kvk.KvKNumber = int.Parse(kvkNumber.Text);
                if (kvk.Id == 0)
                {
                    await db.InsertAsync(kvk);
                }
                else
                {
                    await db.UpdateAsync(kvk);
                }
                user.KvKId = kvk.Id;
                user.BankaccountNumber = bankNumber.Text;
                user.EmailAddress = email.Text;
                user.VATNumber = vatNumber.Text;
                user.PhoneNumber = mobileNumber.Text;
                user.UserFirstName = firstName.Text;
                user.UserLastName = lastName.Text;
                user.Website = website.Text;

                if(user.UserId == 0)
                {
                    await db.InsertAsync(user);
                }
                else
                {
                    await db.UpdateAsync(user);
                }

                await DisplayAlert("Succes", "Instellingen succesvol opgeslagen.", "OK");
            });
        }
    }
}