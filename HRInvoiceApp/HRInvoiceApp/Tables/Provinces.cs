﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace HRInvoiceApp.Tables
{
    public class Provinces
    {
        [PrimaryKey]
        public string Province
        {
            get;
            set;
        }
    }
}