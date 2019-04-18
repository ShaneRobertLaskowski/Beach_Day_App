using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace beach_day
{
    public class ItemListDatabase
    {
        readonly SQLiteAsyncConnection itemListDatabase;

        public ItemListDatabase(string databasePath)
        {
            itemListDatabase = new SQLiteAsyncConnection(databasePath);
            itemListDatabase.CreateTableAsync<ChecklistItemModel>().Wait();
        }
    }


}
