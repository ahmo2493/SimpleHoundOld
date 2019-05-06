using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuProgect1.SQLDatabase
{
    public class MenuKitchenOrder
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
        public string Customer { get; set; }
        public int TableNum { get; set; }
        public string Category { get; set; }
        public string FoodItem { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
        public int Quantity { get; set; }
        public string Password { get; set; }
    }
}
