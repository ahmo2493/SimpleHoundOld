using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleHound.SQLDatabase
{
    public class MenuEntry
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Tables { get; set; }
        public string Categories { get; set; }
        public string Items { get; set; }
        public decimal Prices { get; set; }
    }
}
