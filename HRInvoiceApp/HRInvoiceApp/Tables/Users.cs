using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace HRInvoiceApp.Tables
{
    public class Users
    {
        [PrimaryKey, AutoIncrement]
        public int UserId
        {
            get;
            set;
        }
        [ForeignKey(typeof(KvK))]
        public int KvKNumber
        {
            get;
            set;
        }
        public string UserFirstName
        {
            get;
            set;
        }
        public string UserLastName
        {
            get;
            set;
        }
        public string BankaccountNumber
        {
            get;
            set;
        }
        public int VATNumber
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
}
}
