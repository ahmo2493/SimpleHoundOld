using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleHound.SQLDatabase;

namespace SimpleHound.MyLists
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
