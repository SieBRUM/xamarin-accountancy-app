using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace HRInvoiceApp.Tables
{
    [Table("Provinces")]
    public class Province
    {
        [PrimaryKey, NotNull]
        public string ProvinceName
        {
            get;
            set;
        }
    }
}
