using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace HRInvoiceApp.Helpers
{
    /// <summary>
    /// General class for checking input
    /// </summary>
    public static class InputValidationHelper
    {
        // Ref: https://www.codeproject.com/Tips/775696/IBAN-Validator
        public static bool ValidateIban(string iban)
        {
            iban = iban.ToUpper();
            if (String.IsNullOrEmpty(iban))
            {
                return false;
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(iban, "^[A-Z0-9]"))
            {
                iban = iban.Replace(" ", String.Empty);
                string bank =
                iban.Substring(4, iban.Length - 4) + iban.Substring(0, 4);
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
            {
                return false;
            }
        }

        public static bool ValidateVATNumber(string VATNumber)
        {
            if (String.IsNullOrWhiteSpace(VATNumber))
            {
                return false;
            }
            if (VATNumber.Count() != 14)
            {
                return false;
            }
            if (!VATNumber.StartsWith("NL"))
            {
                return false;
            }

            return true;
        }

        public static bool IsValidEmail(string emailaddress)
        {
            if (!emailaddress.Contains("."))
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

        public static bool IsZipCodeValid(string zipcode)
        {
            String AllowedChars = @"^[a-zA-Z0-9]*$";

            if (Regex.IsMatch(zipcode, AllowedChars))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
