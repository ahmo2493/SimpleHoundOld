using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenuProgect1.SQLDatabase;

namespace MenuProgect1.MyLists
{
    public class FoodLists
    {
        public List<string> CategoryList = new List<string>();
        public List<MenuEntry> FoodItemList = new List<MenuEntry>();
        public List<MenuCustomerOrder> WaiterItemEntryList = new List<MenuCustomerOrder>();
        public List<MenuCustomerOrder> CustomerFoodList = new List<MenuCustomerOrder>();
        public List<MenuKitchenOrder> SendTokitchenList = new List<MenuKitchenOrder>();



    }
}
