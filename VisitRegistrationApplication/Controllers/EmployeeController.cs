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
            using (DentistDBEntities3 objDentistDBEntities = new DentistDBEntities3())
            {
                var user = objDentistDBEntities.Users.Where(a => a.Email == User.Identity.Name).FirstOrDefault();
                ViewBag.RoleId = user.Role;
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
                using (DentistDBEntities3 objDentistDBEntities = new DentistDBEntities3())
                {
                    var objUser = new DBModel.Users();

                    if (objDentistDBEntities.Users.Any(x => x.Email == objUserModel.Email))
                    {
                        ViewBag.ErrorEmailMessage = "Email już istnieje!";
                        return View();
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
            return View();
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            using (DentistDBEntities3 objDentistDBEntities = new DentistDBEntities3())
            {
                return View(objDentistDBEntities.Users.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (DentistDBEntities3 objDentistDBEntities = new DentistDBEntities3())
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
            using (DentistDBEntities3 objDentistDBEntities = new DentistDBEntities3())
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
            var dateInt = startDate.AddHours(Convert.ToInt32(time)).ToString();
            var idUser = Convert.ToInt32(UserId);
            var idEmployee = Convert.ToInt32(EmployeeId);      

            using (DentistDBEntities3 objDentistDBEntities = new DentistDBEntities3())
            {
                var employee = objDentistDBEntities.Users.Where(x => x.Id == idEmployee).FirstOrDefault();

                if (objDentistDBEntities.Visit.Any(x => x.employeeId == idEmployee && x.timedate == dateInt))
                {
                    
                    ViewBag.EmployeeId = employee.Id;
                    ViewBag.EmployeeFirstName = employee.FirstName;
                    ViewBag.EmployeeLastName = employee.LastName;

                    ViewBag.UserId = idUser;
                    ViewBag.ErrorEmailMessage = "Termin już jest zajęty!";
                    return View();
                }

                var objvisit = new DBModel.Visit();
                objvisit.timedate = dateInt;
                objvisit.userId = idUser;
                objvisit.employeeId = idEmployee;
                objvisit.timeOrderBy = startDate.AddHours(Convert.ToInt32(time));
                objvisit.employeeName = employee.FirstName + " " + employee.LastName;

                objDentistDBEntities.Visit.Add(objvisit);
                objDentistDBEntities.SaveChanges();
            }
            ViewBag.ErrorEmailMessage = startDate.AddHours(Convert.ToInt32(time)).Ticks;

            return RedirectToAction("Index", "Employee");
        }

        [Authorize]
        public ActionResult Appointments()
        {
            using (DentistDBEntities3 objDentistDBEntities = new DentistDBEntities3())
            {
                var user = objDentistDBEntities.Users.Where(a => a.Email == User.Identity.Name).FirstOrDefault();
                ViewBag.RoleId = user.Role;
                if (user.Role == 3)
                {
                    return View(objDentistDBEntities.Visit.Where(x => x.employeeId == user.Id).OrderBy(s => s.timeOrderBy).ToList());
                }
                return View(objDentistDBEntities.Visit.Where(x => x.userId == user.Id).OrderBy(s => s.timeOrderBy).ToList());
            }
        }

        [Authorize]
        public ActionResult AppointmentsDelete(int id)
        {
            using (DentistDBEntities3 objDentistDBEntities = new DentistDBEntities3())
            {
                return View(objDentistDBEntities.Visit.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult AppointmentsDelete(int id, FormCollection collection)
        {
            using (DentistDBEntities3 objDentistDBEntities = new DentistDBEntities3())
            {
                var appointment = objDentistDBEntities.Visit.Where(x => x.Id == id).FirstOrDefault();
                objDentistDBEntities.Visit.Remove(appointment);
                objDentistDBEntities.SaveChanges();

                return RedirectToAction("Appointments", "Employee");
            }
        }
    }
}