using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace beach_day.Models
{
    public class BeachPlace
    {
        [Unique]
        public string Name { get; set; }

        //Latitude and Longitude could be combined into a Posistion object, which contains lat/lng values, 
        //allowing for Unique constraint
        public double Latitude { get; set; } 

        public double Longitude { get; set; }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
    }
}
