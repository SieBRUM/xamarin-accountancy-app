using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace HRInvoiceApp.Tables
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int UserId
        {
            get;
            set;
        }
        [ForeignKey(typeof(KvK)), NotNull]
        public int KvKNumber
        {
            get;
            set;
        }
        [NotNull]
        public string UserFirstName
        {
            get;
            set;
        }
        [NotNull]
        public string UserLastName
        {
            get;
            set;
        }
        [NotNull]
        public string BankaccountNumber
        {
            get;
            set;
        }
        [NotNull]
        public int VATNumber
        {
            get;
            set;
        }
        [NotNull]
        public string PhoneNumber
        {
            get;
            set;
        }
        public string Website
        {
            get;
            set;
        }
    }
}
