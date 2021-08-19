using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SimpleHound.SQLDatabase;

namespace SimpleHound.Controllers
{


    public class DashboardController : Controller
    {

        public static List<MenuEmployees> EmployeeList = new List<MenuEmployees>();


        public IActionResult HomeDashboard(int id)
        {
            ViewData["TableNum"] = id;
            return View();
        }

        //--------------------------------------------------------------------------------

        [HttpGet]
        public IActionResult AddEmployees()
        {
           
            using (var db = new MenuDBContext())
            {
               EmployeeList.Clear();
                var contentUser = db.MenuEmployees.Where(x => x.UserName == User.Identity.Name);
                foreach (var item in contentUser)
                {
                    EmployeeList.Add(new MenuEmployees { UserName = item.UserName, Position = item.Position, Name = item.Name, Password = item.Password });
                }
            }

            return View(EmployeeList);
        }

        [HttpPost]
        public IActionResult AddEmployees(string EmployeeName, string DeleteItem, string position, string addWaiter)
        {
            if (addWaiter == "waiter" && EmployeeName == null || addWaiter == "waiter" && position == null)
            {
                ViewData["StaffError"] = "* All fields must be filled *";
            }
            else
            {
                //Deletes from database and list

                if (DeleteItem != null)
                {
                    int.TryParse(DeleteItem, out int deleteNum);

                    using (var db = new MenuDBContext())
                    {
                        db.MenuEmployees.RemoveRange(db.MenuEmployees.Where(x => x.Password == EmployeeList[deleteNum].Password));

                        db.SaveChanges();
                    }

                    EmployeeList.RemoveAt(deleteNum);
                }
                else
                {
                    Random r = new Random();
                    int randomPassword = r.Next(1000, 9000);

                    string myPass = EmployeeName + randomPassword.ToString();

                    MenuEmployees theEmployee = new MenuEmployees() { UserName = User.Identity.Name, Position = position, Name = EmployeeName, Password = myPass.ToUpper() };
                    EmployeeList.Insert(0, theEmployee);


                    using (var db = new MenuDBContext())
                    {
                        db.MenuEmployees.Add(
                            theEmployee);

                        db.SaveChanges();
                    }

                }
            }



            return View(EmployeeList);
        }

        //----------------------------------------------------------------------------------------------------
        

       

    }
}