using HRInvoiceApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

            Task.Run(async () =>
            {
                List<Province> provinces = await database.Table<Province>().ToListAsync();
                if(provinces.Count != 12)
                {
                    InsertProvinces();
                }
            });
        }

        public SQLiteAsyncConnection GetInstance()
        {
            return database;
        }

        private void InsertProvinces()
        {
            database.DeleteAllAsync<Province>();
            List<Province> provinces = new List<Province>();
            provinces.Add(new Province() { ProvinceName= "Groningen" });
            provinces.Add(new Province() { ProvinceName= "Fryslân" });
            provinces.Add(new Province() { ProvinceName= "Drenthe" });
            provinces.Add(new Province() { ProvinceName= "Overijssel" });
            provinces.Add(new Province() { ProvinceName= "Flevoland" });
            provinces.Add(new Province() { ProvinceName= "Utrecht" });
            provinces.Add(new Province() { ProvinceName= "Gelderland" });
            provinces.Add(new Province() { ProvinceName= "Noord-Holland" });
            provinces.Add(new Province() { ProvinceName= "Zuid-Holland" });
            provinces.Add(new Province() { ProvinceName= "Zeeland" });
            provinces.Add(new Province() { ProvinceName= "Noord-Brabant" });
            provinces.Add(new Province() { ProvinceName= "Limburg" });
            database.InsertAllAsync(provinces);
        }
    }
}
