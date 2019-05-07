
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuProgect1.SQLDatabase
{
    public class MenuDBContext : DbContext
    {
       
        public DbSet<MenuEntry> MenuEntry { get; set; }
        public DbSet<MenuEmployees> MenuEmployees { get; set; }
        public DbSet<MenuCustomerOrder> MenuCustomerOrder { get; set; }
        public DbSet<MenuKitchenOrder> MenuKitchen { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server= MyCode");
        }
    }
}
