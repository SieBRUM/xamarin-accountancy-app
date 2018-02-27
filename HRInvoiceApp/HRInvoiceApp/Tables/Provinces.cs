using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace HRInvoiceApp.Tables
{
    public class Provinces
    {
        [PrimaryKey, AutoIncrement]
        public string Province
        {
            get;
            set;
        }
    }
}
