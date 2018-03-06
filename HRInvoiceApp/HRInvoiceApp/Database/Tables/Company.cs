using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;


namespace HRInvoiceApp.Tables
{
    [Table("Companies")]
    public class Company
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int CompanyId
        {
            get;
            set;
        }
        [ForeignKey(typeof(KvK)), NotNull]
        public int KvKId
        {
            get;
            set;
        }
        [ForeignKey(typeof(Province)), NotNull]
        public string Province
        {
            get;
            set;
        }
        [ForeignKey(typeof(User)), NotNull]
        public int UserId
        {
            get;
            set;
        }
        [NotNull]
        public string CompanyName
        {
            get;
            set;
        }
        public string AddressAddition
        {
            get;
            set;
        }
        [NotNull]
        public string City
        {
            get;
            set;
        }
        [NotNull]
        public string Zipcode
        {
            get;
            set;
        }
    }
}
