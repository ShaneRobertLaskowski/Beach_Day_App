using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace beach_day
{
    public class ChecklistItemModel
    {
        [PrimaryKey]
        public string Name { get; set; }
        [AutoIncrement]
        public int ID { get; set; }
    }
}
