using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuProgect1.SQLDatabase
{
    public class MenuEmployees
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Position { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
