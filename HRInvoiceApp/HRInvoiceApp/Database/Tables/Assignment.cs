using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

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
        [ForeignKey(typeof(Department)), NotNull]
        public int DepartmentId
        {
            get;
            set;
        }
        [NotNull]
        public float HourSalary
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

        public string fullName
        {
            get
            {
                return $"{ClientFirstName} {ClientLastName} ({CostCenterNumber})";
            }
        }
    }
}
