using HRInvoiceApp.Tables;
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
            database.CreateTablesAsync<User, Company>().Wait();
            database.CreateTablesAsync<Assignment, Department>().Wait();
            database.CreateTablesAsync<KvK, Province>().Wait();
            database.CreateTableAsync<Workday>().Wait();
        }

        public SQLiteAsyncConnection GetInstance()
        {
            return database;
        }
    }
}
