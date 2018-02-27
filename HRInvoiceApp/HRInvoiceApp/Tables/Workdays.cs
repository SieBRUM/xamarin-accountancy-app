using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace HRInvoiceApp.Tables
{
    public class Workdays
    {
        [PrimaryKey, AutoIncrement]
        public int WorkdayId
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
        public int ExtraExpenses
        {
            get;
            set;
        }
        public string note
        {
            get;
            set;
        }
        public DateTime Date
        {
            get;
            set;
        }
    }
}
