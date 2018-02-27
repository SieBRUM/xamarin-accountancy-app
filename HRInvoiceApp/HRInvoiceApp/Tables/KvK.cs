using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRInvoiceApp.Tables
{
    public class KvK
    {
        [PrimaryKey]
        public int KvKNumber
        {
            get;
            set;
        }
    }
}
