using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRInvoiceApp.Database
{
    public class AccountancyDatabase
    {
        readonly SQLiteAsyncConnection database;

        public AccountancyDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
        }

        public SQLiteAsyncConnection GetInstance()
        {
            return database;
        }
    }
}
