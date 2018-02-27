using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRInvoiceApp.Tables
{
    public class Departments
    {
        [PrimaryKey, AutoIncrement]
        public int DepartmentId
        {
            get;
            set;
        }
        [ForeignKey(typeof(CostCenterNumbers))]
        public int CostCenterNumber
        {
            get;
            set;
        }
        public string DepartmentName
        {
            get;
            set;
        }
        public int BankaccountNumber
        {
            get;
            set;
        }
    }
}
