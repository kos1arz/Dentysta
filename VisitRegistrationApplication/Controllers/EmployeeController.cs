using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisitRegistrationApplication.DBModel;
using VisitRegistrationApplication.Models;

namespace VisitRegistrationApplication.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        [Authorize]
        public ActionResult Index()
        {
            using (DentistDBEntities objDentistDBEntities = new DentistDBEntities())
            {
                return View(objDentistDBEntities.Users.Where(x => x.Role == 3).ToList());
            }
        }

        [Authorize]
        public ActionResult Create()
        {
            UserModel objUserModel = new UserModel();
            return View(objUserModel);

        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(UserModel objUserModel)
        {
            if (ModelState.IsValid)
            {
                using (DentistDBEntities objDentistDBEntities = new DentistDBEntities())
                {
                    User objUser = new DBModel.User();

                    if (objDentistDBEntities.Users.Any(x => x.Email == objUserModel.Email))
                    {
                        ViewBag.ErrorEmailMessage = "Email już istnieje!";
                        return View("Create", "Employee");
                    }

                    objUser.CreatedOn = DateTime.Now;
                    objUser.FirstName = objUserModel.FirstName;
                    objUser.LastName = objUserModel.LastName;
                    objUser.Email = objUserModel.Email;
                    objUser.Password = Hash.HashPassword(objUserModel.Password);
                    objUser.Role = 3;

                    objDentistDBEntities.Users.Add(objUser);
                    objDentistDBEntities.SaveChanges();

                    return RedirectToAction("Index", "Employee");
                }
            }
            return View("Create", "Employee");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            using (DentistDBEntities objDentistDBEntities = new DentistDBEntities())
            {
                return View(objDentistDBEntities.Users.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (DentistDBEntities objDentistDBEntities = new DentistDBEntities())
            {
                var user = objDentistDBEntities.Users.Where(x => x.Id == id).FirstOrDefault();
                objDentistDBEntities.Users.Remove(user);
                objDentistDBEntities.SaveChanges();

                return RedirectToAction("Index", "Employee");
            }
        }

        // GET: Employee
        [Authorize]
        [HttpGet]
        public ActionResult Visit(int id)
        {
            using (DentistDBEntities objDentistDBEntities = new DentistDBEntities())
            {
               var employee = objDentistDBEntities.Users.Where(x => x.Id == id).FirstOrDefault();
                var user = objDentistDBEntities.Users.Where(a => a.Email == User.Identity.Name).FirstOrDefault();

                ViewBag.EmployeeId = employee.Id;
                ViewBag.EmployeeFirstName = employee.FirstName;
                ViewBag.EmployeeLastName = employee.LastName;

                ViewBag.UserId = user.Id;

                return View();
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Visit(string UserId, string EmployeeId, string time, DateTime startDate)
        {
            // ViewBag.ErrorEmailMessage = startDate.Ticks;
            ViewBag.ErrorEmailMessage = startDate.AddHours(Convert.ToInt32(time));

            return View();
        }
    }
}