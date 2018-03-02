using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRInvoiceApp.Tables
{
    public class KvK
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id
        {
            get;
            set;
        }

        [NotNull]
        public int KvKNumber
        {
            get;
            set;
        }
    }
}
