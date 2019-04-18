using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace beach_day
{
    class ChecklistItemModel
    {
        [PrimaryKey]
        public string Name { get; set; }
        [AutoIncrement]
        public string ID { get; set; }
    }
}
