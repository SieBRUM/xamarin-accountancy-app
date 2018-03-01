using HRInvoiceApp.Database;
using HRInvoiceApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HRInvoiceApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
        SQLiteAsyncConnection db;
        User user;

        public SettingsPage()
		{
			InitializeComponent();
            db = App.Database.GetInstance();
            var getUserTask = db.Table<User>().FirstOrDefaultAsync();
            Task.WaitAny(getUserTask);
            user = getUserTask.Result;
        }

        void onSaveClicked(object sender, EventArgs e)
        {
            foreach (var view in settingsStackLayout.Children)
            {
                if(view is Entry)
                {
                    Entry entry = (Entry)view;
                    if(entry.Placeholder != "Website" && string.IsNullOrWhiteSpace(entry.Text))
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
            if (!IsValidEmail(email.Text))
            {
                DisplayAlert("Alert", "Graag een correct email adres nummer invullen.", "OK");
                return;
            }
            if(!ValidateIban(bankNumber.Text))
            {
                DisplayAlert("Alert", "Graag een correct IBAN nummer invullen.", "OK");
                return;
            }

            if(user != null)
            {
                Task<KvK> getKvk = db.Table<KvK>().Where(x => x.KvKNumber == user.KvKNumber).FirstAsync();
                Task.WaitAny(getKvk);
                KvK excistingKvk = getKvk.Result;
                //excistingKvk.KvKNumber = kvkNumber.Text;
            }

            var insertKvk = db.InsertAsync(new KvK() { KvKNumber = int.Parse(kvkNumber.Text) });
            Task.WaitAny(insertKvk);
            var kvk = insertKvk.Result;
            if(user == null)
            {
                user = new User();
            }
            user.KvKNumber = kvk;
            user.BankaccountNumber = bankNumber.Text;
            user.UserFirstName = firstName.Text;
            user.UserLastName = lastName.Text;
            user.VATNumber = int.Parse(vatNumber.Text);
            user.Website = website.Text;
            user.PhoneNumber = mobileNumber.Text;
        }

        public bool IsValidEmail(string emailaddress)
        {
            if(!emailaddress.Contains("."))
            {
                return false;
            }

            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        // Ref: https://www.codeproject.com/Tips/775696/IBAN-Validator
        bool ValidateIban(string bankNumber)
        {
            bankNumber = bankNumber.ToUpper();
            if (String.IsNullOrEmpty(bankNumber))
                return false;
            else if (System.Text.RegularExpressions.Regex.IsMatch(bankNumber, "^[A-Z0-9]"))
            {
                bankNumber = bankNumber.Replace(" ", String.Empty);
                string bank =
                bankNumber.Substring(4, bankNumber.Length - 4) + bankNumber.Substring(0, 4);
                int asciiShift = 55;
                StringBuilder sb = new StringBuilder();
                foreach (char c in bank)
                {
                    int v;
                    if (Char.IsLetter(c)) v = c - asciiShift;
                    else v = int.Parse(c.ToString());
                    sb.Append(v);
                }
                string checkSumString = sb.ToString();
                int checksum = int.Parse(checkSumString.Substring(0, 1));
                for (int i = 1; i < checkSumString.Length; i++)
                {
                    int v = int.Parse(checkSumString.Substring(i, 1));
                    checksum *= 10;
                    checksum += v;
                    checksum %= 97;
                }
                return checksum == 1;
            }
            else
                return false;
        }
    }
}