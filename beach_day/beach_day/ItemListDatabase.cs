using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using beach_day.Models;

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

        public Task<int> SaveItemAsync(ChecklistItemModel item)
        {
            //*****need to run query to check if item.Name already in DB, use if else or maybe try-catch
            return itemListDatabase.InsertAsync(item);

        }

        public Task<List<ChecklistItemModel>> GetAllChecklistItems()
        {
            return itemListDatabase.QueryAsync<ChecklistItemModel>("SELECT * FROM [ChecklistItemModel]");
        }

        public Task<int> DeleteItemAsync(ChecklistItemModel item)
        {
            return itemListDatabase.DeleteAsync(item);
        }

    }
}
