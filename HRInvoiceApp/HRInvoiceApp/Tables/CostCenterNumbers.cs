using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace HRInvoiceApp.Tables
{
    public class CostCenterNumbers
    {
        [PrimaryKey]
        public int CostCenterNumber
        {
            get;
            set;
        }
        public int HourSalary
        {
            get;
            set;
        }
        public string ClientFirstName
        {
            get;
            set;
        }
        public string ClientLastName
        {
            get;
            set;
        }
        public string ClientEmailAddress
        {
            get;
            set;
        }
        public string ClientMobileNumber
        {
            get;
            set;
        }
    }
}
