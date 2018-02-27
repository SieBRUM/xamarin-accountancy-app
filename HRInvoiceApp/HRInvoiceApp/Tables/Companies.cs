using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;


namespace HRInvoiceApp.Tables
{
    public class Companies
    {
        [PrimaryKey, AutoIncrement]
        public int CompanyId
        {
            get;
            set;
        }
        [ForeignKey(typeof(Departments))]
        public int DepartmentId
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
        [ForeignKey(typeof(Provinces))]
        public string Province
        {
            get;
            set;
        }
        [ForeignKey(typeof(Users))]
        public int UserId
        {
            get;
            set;
        }

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
        public string City
        {
            get;
            set;
        }
        public string Zipcode
        {
            get;
            set;
        }
    }
}
