using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenuProgect1.MyLists;
using MenuProgect1.SQLDatabase;

namespace MenuProgect1.Logic
{
    public class Helper
    {
        //******************************************* Home Controller *******************************************************************************


        //Menu food items will be retrieved from database if user login is recognized ( MenuFoodEntry - HttpGet )

        public static FoodLists AddCategoryandFoodItems(IQueryable<MenuEntry> containsUser, FoodLists FoodLists)
        {

            foreach (var item in containsUser)
            {
                FoodLists.CategoryList.Add(item.Categories);
                FoodLists.FoodItemList.Add(new MenuEntry() { UserName = item.UserName, Tables = item.Tables, Categories = item.Categories, Items = item.Items, Prices = item.Prices });
            }
            return FoodLists;
        }


        //Error Messages ( MenuFoodEntry - HttpPost )

        public static string CategoryErrorMessage(string addCategory, string EntryBoxCategory)
        {
            string categoryError = "";

            if (addCategory != null && EntryBoxCategory == null)
            {
                 categoryError = "*Category must be filled*";
                
            }
            return categoryError;
        }


        public static string FoodItemError(string addFoodItem, string EntryBoxItem, string EntryBoxPrice)
        {
            string foodItemError = "";
            if (addFoodItem != null && EntryBoxItem == null || addFoodItem != null && EntryBoxPrice == null)
            {
                foodItemError = "*All fields must be filled*";
            }
            return foodItemError;
        }

        //When you delete a category all the food items inside will be deleted as well

        public static void DeleteFoodItemsInsideCategory(FoodLists FoodLists, int deleteNum,MenuEntry item)
        {   
                if (FoodLists.CategoryList[deleteNum] == item.Categories)
                {

                    FoodLists.FoodItemList.Remove(item);
                }       
        }
        
        public static int RetreaveDatabaseTableNum(IQueryable<MenuEntry> fixZero, int tableNum)
        {
            foreach (var item in fixZero)
            {
                tableNum = item.Tables;
            }
            return tableNum;
        }


        //******************************************* Employee Controller ***************************************************************************

        // Encrypt & Decrypt Password 

        public static string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }

        public static string Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }

        //Kitchen-UI adding fooditems KitchenFulfillmentList

        public static void AddTofillmentList(IQueryable <MenuKitchenOrder> foodSent, List <MenuKitchenOrder> KitchenFulfillmentList)
        {
            foreach (var item in foodSent)
            {
                KitchenFulfillmentList.Add(new MenuKitchenOrder { Id = item.Id, UserName = item.UserName, EmployeeName = item.EmployeeName, Customer = item.Customer, TableNum = item.TableNum, Category = item.Category, FoodItem = item.FoodItem, Price = item.Price, Note = item.Note, Quantity = item.Quantity, Password = item.Password });
            }
        }

        //Kitchen-UI Deleting kitchen food item

        public static void DeleteById(MenuKitchenOrder item, int deleteItem, List<MenuKitchenOrder> KitchenFulfillmentList)
        {
            if (item.Id == deleteItem)
            {
                KitchenFulfillmentList.Remove(item);
            }
        }

        //WaiterTableEntry 
        public static void UpdateTableNumber(IQueryable <MenuCustomerOrder> foodItems, int myTable)
        {
            foreach (var item in foodItems)
            {
                item.TableNum = myTable;
            }
        }

        //WaiterCustomerEntry
        public static List<MenuCustomerOrder> AddToCustomerFixedList(IQueryable <MenuCustomerOrder> customerCount, List <MenuCustomerOrder> CustomerFixedList)
        {
            foreach (var item in customerCount)
            {
                CustomerFixedList.Add(new MenuCustomerOrder() { Customer = item.Customer, Password = item.Password });
            }
            return CustomerFixedList;
        }

       
    }
}
