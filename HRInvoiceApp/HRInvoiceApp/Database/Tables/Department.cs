using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRInvoiceApp.Tables
{
    [Table("Departments")]
    public class Department
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int DepartmentId
        {
            get;
            set;
        }
        [ForeignKey(typeof(Assignment)), NotNull]
        public int CostCenterNumber
        {
            get;
            set;
        }
        [ForeignKey(typeof(Company)), NotNull]
        public int CompanyId
        {
            get;
            set;
        }

        [NotNull]
        public string DepartmentName
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
    }
}
