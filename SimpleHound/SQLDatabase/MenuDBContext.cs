﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleHound.SQLDatabase
{
    public class MenuDBContext : DbContext
    {
       
        public DbSet<MenuEntry> MenuEntry { get; set; }
        public DbSet<MenuEmployees> MenuEmployees { get; set; }
        public DbSet<MenuCustomerOrder> MenuCustomerOrder { get; set; }
        public DbSet<MenuKitchenOrder> MenuKitchen { get; set; }
        public DbSet<CustomerCount> CustomerCount { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=tcp:amolearnserver.database.windows.net,1433;Initial Catalog=amoLearnDatabase;Persist Security Info=False;User ID=;Password=;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}
