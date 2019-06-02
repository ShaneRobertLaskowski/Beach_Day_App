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
            //Should handle check if item.Name already in DB, use if else or maybe try-catch
            return itemListDatabase.InsertAsync(item); //Code explodes with "Contraint" exception
        }

        public Task<List<ChecklistItemModel>> GetAllChecklistItems()
        {
            return itemListDatabase.QueryAsync<ChecklistItemModel>("SELECT * FROM [ChecklistItemModel]");
        }

        public Task<int> DeleteItemAsync(ChecklistItemModel item)
        {
            return itemListDatabase.DeleteAsync(item);
        }

        public Task<List<ChecklistItemModel>> GetItemWithName(string itemName)
        {
            //var table = (TableMapping)itemListDatabase.TableMappings; //grabs the currenlty understood table (i hope its the right one!)
            string query = "SELECT * FROM [ChecklistItemModel] WHERE [Name] = \"" + itemName + "\"";

            return itemListDatabase.QueryAsync<ChecklistItemModel>(query);

        }

    }
}
