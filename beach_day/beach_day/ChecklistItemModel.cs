using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace beach_day
{
    public class ChecklistItemModel
    {
        [Unique]
        public string Name { get; set; }
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }


    }
}
