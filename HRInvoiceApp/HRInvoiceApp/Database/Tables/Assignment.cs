using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace HRInvoiceApp.Tables
{
    [Table("Assignments")]
    public class Assignment
    {
        [PrimaryKey, NotNull]
        public int CostCenterNumber
        {
            get;
            set;
        }
        [NotNull]
        public int HourSalary
        {
            get;
            set;
        }
        [NotNull]
        public string ClientFirstName
        {
            get;
            set;
        }
        [NotNull]
        public string ClientLastName
        {
            get;
            set;
        }
        [NotNull]
        public string ClientEmailAddress
        {
            get;
            set;
        }
        [NotNull]
        public string ClientMobileNumber
        {
            get;
            set;
        }
    }
}
