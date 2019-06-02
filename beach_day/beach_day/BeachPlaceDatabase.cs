using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using beach_day.Models;

namespace beach_day
{

    public class BeachPlaceDatabase
    {
        readonly SQLiteAsyncConnection beachPlaceDatabase;

        public BeachPlaceDatabase(string databasePath)
        {
            beachPlaceDatabase = new SQLiteAsyncConnection(databasePath);
            beachPlaceDatabase.CreateTableAsync<BeachPlace>().Wait();
        }

        public Task<int> SaveItemAsync(BeachPlace item)
        {
            //Should handle check if BeachPlace.Name already in DB, use if else or maybe try-catch
            return beachPlaceDatabase.InsertAsync(item); //Code explodes with "Contraint" exception
        }

        public Task<List<BeachPlace>> GetAllBeachPlaces()
        {
            return beachPlaceDatabase.QueryAsync<BeachPlace>("SELECT * FROM [BeachPlace]");
        }

        public Task<int> DeleteItemAsync(BeachPlace item)
        {
            return beachPlaceDatabase.DeleteAsync(item);
        }

        //****make this return a single BeachPlace, not a list (the BeachPlace's Name has the Unique constraint)
        //create a seperate function that is named "GetBeachesWithName" instead (in case we remove the Unique constraint)
        public Task<List<BeachPlace>> GetBeachWithName(string itemName)
        {
            string query = "SELECT * FROM [BeachPlace] WHERE [Name] = \"" + itemName + "\"";

            return beachPlaceDatabase.QueryAsync<BeachPlace>(query);

        }
    }
}
