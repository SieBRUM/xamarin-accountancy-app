using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace HRInvoiceApp.Tables
{
    [Table("Workdays")]
    public class Workday
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int WorkdayId
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
        [NotNull]
        public int WorkedHours
        {
            get;
            set;
        }
        public int TraveledDistance
        {
            get;
            set;
        }
        public string ExtraExpenses
        {
            get;
            set;
        }
        public string Note
        {
            get;
            set;
        }
        [NotNull]
        public DateTime Date
        {
            get;
            set;
        }
    }
}
