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
    public class UserController : Controller
    {

        // GET: User
        [Authorize]
        public ActionResult Index()
        {
            using (DentistDBEntities3 objDentistDBEntities = new DentistDBEntities3())
            {
                var user = objDentistDBEntities.Users.Where(a => a.Email == User.Identity.Name).FirstOrDefault();
                ViewBag.Identificacion = user.Id;
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.Email = user.Email;
                ViewBag.CreatedOn = user.CreatedOn;

                return View(user);
            }      
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            using (DentistDBEntities3 objDentistDBEntities = new DentistDBEntities3())
            {
                return View(objDentistDBEntities.Users.Where(s => s.Id == id).FirstOrDefault());
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, Users user)
        {
            try
            {
                using (DentistDBEntities3 objDentistDBEntities = new DentistDBEntities3())
                {
                    objDentistDBEntities.Entry(user).State = EntityState.Modified;
                    objDentistDBEntities.SaveChanges();

                    return RedirectToAction("Index", "User");
                }
            } catch
            {
                return View();
            }
        }
    }
}