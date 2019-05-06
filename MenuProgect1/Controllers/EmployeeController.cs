﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MenuProgect1.SQLDatabase;
using MenuProgect1.MyLists;
using MenuProgect1.Logic;

namespace MenuProgect1.Controllers
{


    public class EmployeeController : Controller
    {
        //Customer list is for customer entry only (customer 1, 2, 3...)
        public static List<MenuCustomerOrder> CustomerList = new List<MenuCustomerOrder>();

        //CustomerOrder has all the food items which will be sent to the kitchen
        public static FoodLists CustomerOrder = new FoodLists();

        //List for Kitchen Employees 
        public static List<MenuKitchenOrder> KitchenFulfillmentList = new List<MenuKitchenOrder>();

        [HttpGet]
        public IActionResult Oasis()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Oasis(string OasisPassword)
        {


            using (var db = new MenuDBContext())
            {

                foreach (var item in db.MenuEmployees)
                {
                    if (item.Password == OasisPassword.ToUpper())
                    {
                      string encrypted = Helper.Encode(item.Password);
                        string encryptedGmail = Helper.Encode(item.UserName);

                        if (item.Position == "waiter")
                        {
                            return RedirectToAction("WaiterTableEntry", "Employee", new { id = encrypted, OrderSentMessage = "" });

                        }
                        else if (item.Position == "kitchen")
                        {
                            return RedirectToAction("KitchenUI", "Employee", new { id = encrypted, gmailUser = encryptedGmail });
                        }
                    }
                }

            }
            ViewData["Error"] = "Incorrect Password";

            return View();
        }
        //---------------------------------------- Kitchen UI -------------------------------------------------
        [HttpGet]
        public IActionResult KitchenUI(string id, string gmailUser) 
        {
            string decodedPassword = Helper.Decode(id);
            ViewData["password"] = decodedPassword;

            string decodedGmail = Helper.Decode(gmailUser);

            KitchenFulfillmentList.Clear();

            using(var db = new MenuDBContext())
            {   
                //add all the items in MenuKitchen that have the same UserName to the KitchenFulfillmentList

                var foodSent = from k in db.MenuKitchen
                               where k.UserName == decodedGmail
                               select k;

                Helper.AddTofillmentList(foodSent, KitchenFulfillmentList);
            }

            return View(KitchenFulfillmentList);
        }

        [HttpPost]
        public IActionResult KitchenUI(string id,string gmailUser, string doneButton)
        {
            string decodedPassword = Helper.Decode(id);
            string decodedGmail = Helper.Decode(gmailUser);

            if (doneButton != null)
            {
              
                int deleteItem = int.Parse(doneButton);
  //Delete from list and database
                foreach (var item in KitchenFulfillmentList.ToList())
                {
                    Helper.DeleteById(item, deleteItem, KitchenFulfillmentList);
                }

                using(var db = new MenuDBContext())
                {
                    db.MenuKitchen.RemoveRange(db.MenuKitchen.Where(x => x.Id == deleteItem));

                    db.SaveChanges();
                }
                
                
            }
            return View(KitchenFulfillmentList);
        }



        //---------------------------------------- Table Entry-------------------------------------------------

        [HttpGet]
        public IActionResult WaiterTableEntry(string id, string OrderSentMessage)
        {
              string decodedPassword = Helper.Decode(id);
            
            ViewData["OrderSentMessage"] = OrderSentMessage;
            ViewData["password"] = decodedPassword;
            using (var db = new MenuDBContext())
            {
                var joinQuery = (from a in db.MenuEntry
                                 join b in db.MenuEmployees
                                 on a.UserName equals b.UserName
                                 select new
                                 {
                                     b.UserName,
                                     b.Name,
                                     b.Position,
                                     b.Password,
                                     a.Tables,
                                     a.Categories,
                                     a.Items,
                                     a.Prices,
                                 });
                foreach (var item in joinQuery)
                {
                    if (decodedPassword == item.Password)
                    {
                        ViewData["table"] = item.Tables;
                        ViewData["name"] = item.Name;
                    }
                }

            }

            return View();
        }

        [HttpPost]
        public IActionResult WaiterTableEntry(string id,string OrderSentMessage, string tableSelected)
        {
            string decodedPassword = Helper.Decode(id);
            //If you have food items in your database the table number will be updated

            int myTable = int.Parse(tableSelected);

            if (tableSelected != null)
            {
                using (var db = new MenuDBContext())
                {
                    //Updates MenuCustomerOrder Tables is you go back to select different table 
                    var foodItems = db.MenuCustomerOrder.Where(x => x.Password == decodedPassword);

                    Helper.UpdateTableNumber(foodItems, myTable);

                    db.SaveChanges();
                }
                //Once selected you will be directed to WaiterCustomerEntry
                string encrypted = Helper.Encode(decodedPassword);
                return RedirectToAction("WaiterCustomerEntry", new { id = tableSelected, password = encrypted });
            }
            return View();
        }

        //------------------------------------------------- Customer Entry -------------------------------------------------
        [HttpGet]
        public IActionResult WaiterCustomerEntry(string id, string password)
        {
            ViewData["table"] = id;
            ViewData["encryptedpassword"] = password;

            string decodedPassword = Helper.Decode(password);

            //Retrieve Customer count depending on the password in database
            //Multiple users can use without having same customer count
            //CustomerFixedList is only used for this one Purpose

            List<MenuCustomerOrder> CustomerFixedList = new List<MenuCustomerOrder>();

            using(var db = new MenuDBContext())
            {
                var customerCount = db.MenuCustomerOrder.Where(x => x.Password == decodedPassword);

                //Items added to CustomerFixedList
                CustomerFixedList = Helper.AddToCustomerFixedList(customerCount, CustomerFixedList);
            }
            CustomerList.Clear();
            CustomerList.AddRange(CustomerFixedList);

            return View(CustomerList);
        }

        [HttpPost]
        public IActionResult WaiterCustomerEntry(string id, string password, string addCustomer, string DeleteItem, string customerButton, string submitOrder)
        {
            ViewData["table"] = id;

            string decodedPassword = Helper.Decode(password);          

            string encrypted = Helper.Encode(decodedPassword);
            ViewData["encryptedpassword"] = encrypted;

            ViewData["customerNumber"] = customerButton;
            ViewData["OrderSentMessage"] = $" Tabel {id} order was sent!";

            int.TryParse(DeleteItem, out int DeleteCustomer);


            if (DeleteItem != null)
            {
                //deletes customer from database and list
                using (var db = new MenuDBContext())
                {
                    db.MenuCustomerOrder.RemoveRange(db.MenuCustomerOrder.Where(x => x.Customer == DeleteItem && x.Password == decodedPassword));
                    db.SaveChanges();
                }
                CustomerList.RemoveAt(DeleteCustomer - 1);
            }
            if (submitOrder != null)
            { 
                using (var db = new MenuDBContext())
                {
                    //Add db.MenuCustomerOrder to CustomerOrder.SendToKitchenList
                    var selectOrder = from x in db.MenuCustomerOrder
                                      where x.Password == decodedPassword
                                      select x;


                    foreach (var item in selectOrder)
                    {
                        CustomerOrder.SendTokitchenList.Add(new MenuKitchenOrder { UserName = item.UserName, EmployeeName = item.EmployeeName, Customer = item.Customer, TableNum = item.TableNum, Category = item.Category, FoodItem = item.FoodItem, Price = item.Price, Note = item.Note, Quantity = item.Quantity, Password = item.Password });
                    }

                    //clear db.MenuCustomerOrder Database
                    db.MenuCustomerOrder.RemoveRange(db.MenuCustomerOrder.Where(x => x.Password == decodedPassword));

                    //Add CustomerOrder.SendToKitchenList to db.MenuKitchen
                    //The kitchen side will retriave from db.MenuKitchen 
                    db.MenuKitchen.AddRange(
                         CustomerOrder.SendTokitchenList
                         );
                   
                    db.SaveChanges();
                }
                CustomerOrder.SendTokitchenList.Clear();
                  CustomerList.Clear();
          
                return RedirectToAction("WaiterTableEntry", new { id = encrypted, OrderSentMessage = ViewData["OrderSentMessage"] });

            }
            if (customerButton != null)
            {
                return RedirectToAction("WaiterFoodEntry", new { id = ViewData["customerNumber"], password = encrypted, table = ViewData["table"] });
            }
            if (addCustomer != null)
            {
                CustomerList.Add(new MenuCustomerOrder() { Customer = customerButton, Password = decodedPassword });
            }

            return View(CustomerList);
        }

        //------------------------------------------------- Food Entry ---------------------------------------------------

        [HttpGet]
        public IActionResult WaiterFoodEntry(string id, string password, string table)
        {
            ViewData["customerNumber"] = id;

            string decodedPassword = Helper.Decode(password);
            ViewData["password"] = decodedPassword;
            ViewData["tabelNumber"] = table;

            //Depending on what customer number is clicked, the database items will be retrieved             
            using (var db = new MenuDBContext())
            {
                CustomerOrder.CustomerFoodList.Clear();

                var databaseFood = db.MenuCustomerOrder.Where(x => x.Password == decodedPassword && x.Customer == id);

                foreach (var item in databaseFood)
                {
                    CustomerOrder.CustomerFoodList.Add(new MenuCustomerOrder { Id = item.Id, UserName = item.UserName, EmployeeName = item.EmployeeName, Customer = item.Customer, TableNum = item.TableNum, Category = item.Category, FoodItem = item.FoodItem, Price = item.Price, Note = item.Note, Quantity = item.Quantity, Password = item.Password });
                }
            }


           CustomerOrder.CategoryList.Clear();
            using (var db = new MenuDBContext())
            {
              
                var joinQuery = (from a in db.MenuEntry
                                 join b in db.MenuEmployees
                                 on a.UserName equals b.UserName
                                 select new
                                 {
                                     b.UserName,
                                     b.Name,
                                     b.Position,
                                     b.Password,
                                     a.Tables,
                                     a.Categories,
                                     a.Items,
                                     a.Prices,
                                 });
                foreach (var item in joinQuery)
                {
                    if (decodedPassword == item.Password)
                    {

                        CustomerOrder.CategoryList.Add(item.Categories);
                    }
                }
            }
            //deletes any duplicate categories and adds the new list with no repeating categories

            List<string> disinctCategory = CustomerOrder.CategoryList.Distinct().ToList();
            CustomerOrder.CategoryList.Clear();
            CustomerOrder.CategoryList.AddRange(disinctCategory);

            return View(CustomerOrder);
        }


        [HttpPost]
        public IActionResult WaiterFoodEntry(string id, string password, string table, string categorySelected, string foodItemSelected, string backButton, string deleteFoodItem, string qty, string noteTextBox, string noteAdd)
        {
            ViewData["customerNumber"] = id;
            string customerNum = id;
            ViewData["seclectedCategory"] = "";

            string decodedPassword = Helper.Decode(password);
            ViewData["password"] = decodedPassword;

            ViewData["tabelNumber"] = table;

            int.TryParse(deleteFoodItem, out int itemPosition);
            int.TryParse(table, out int myTable);



            if (backButton == "pressed")
            {
                using (var db = new MenuDBContext())
                {
                    db.MenuCustomerOrder.RemoveRange(db.MenuCustomerOrder.Where(x => x.Password == decodedPassword && x.Customer == customerNum));

                    db.MenuCustomerOrder.AddRange(
                        CustomerOrder.CustomerFoodList
                        );
               
                    db.SaveChanges();
                }

                CustomerOrder.CustomerFoodList.Clear();

                string encrypted = Helper.Encode(decodedPassword);
                return RedirectToAction("WaiterCustomerEntry", new { id = ViewData["tabelNumber"], password = encrypted });
            }
            else
            {
                if (categorySelected != null)
                {
                    CustomerOrder.WaiterItemEntryList.Clear();
                    using (var db = new MenuDBContext())
                    {
                        var joinQuery = (from a in db.MenuEntry
                                         join b in db.MenuEmployees
                                         on a.UserName equals b.UserName
                                         select new
                                         {
                                             b.UserName,
                                             b.Name,
                                             b.Position,
                                             b.Password,
                                             a.Tables,
                                             a.Categories,
                                             a.Items,
                                             a.Prices,
                                         });
                        foreach (var item in joinQuery)
                        {
                            if (categorySelected == item.Categories && item.Password == decodedPassword)
                            {

                                CustomerOrder.WaiterItemEntryList.Add(new MenuCustomerOrder { UserName = item.UserName, EmployeeName = item.Name, Customer = customerNum, TableNum = myTable, Category = categorySelected, FoodItem = item.Items, Price = item.Prices });
                                ViewData["seclectedCategory"] = item.Categories;

                            }

                        }

                    }

                }
                if (foodItemSelected != null)
                {
                    foreach (var Fooditem in CustomerOrder.WaiterItemEntryList)
                    {
                        if (foodItemSelected == Fooditem.FoodItem)
                        {
                            CustomerOrder.CustomerFoodList.Add(new MenuCustomerOrder { UserName = Fooditem.UserName, EmployeeName = Fooditem.EmployeeName, Customer = customerNum, TableNum = myTable, Category = Fooditem.Category, FoodItem = Fooditem.FoodItem, Price = Fooditem.Price, Note = "", Quantity = 1, Password = decodedPassword });
                            ViewData["seclectedCategory"] = Fooditem.Category;
                        }
                    }

                }
                if (deleteFoodItem != null)
                {

                    CustomerOrder.CustomerFoodList.RemoveAt(itemPosition);

                }
                if (qty != null)
                {
                    string selectedQty = qty.Substring(2);
                    string qtyNum = qty.Substring(0, 1);
                    int qtyPosition = int.Parse(qtyNum);

                    int num = CustomerOrder.CustomerFoodList[qtyPosition].Quantity;
                    switch (selectedQty)
                    {
                        case "up":

                            CustomerOrder.CustomerFoodList[qtyPosition].Quantity = ++num;
                            break;

                        case "down":
                            CustomerOrder.CustomerFoodList[qtyPosition].Quantity = --num;
                            break;
                    }
                    if (CustomerOrder.CustomerFoodList[qtyPosition].Quantity < 1)
                    {
                        CustomerOrder.CustomerFoodList[qtyPosition].Quantity = 1;
                    }
                }
                if (noteAdd != null)
                {
                    int location = int.Parse(noteAdd);
                    CustomerOrder.CustomerFoodList[location].Note = noteTextBox;

                }

            }

            return View(CustomerOrder);
        }
    }
}