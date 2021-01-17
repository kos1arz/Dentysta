using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VisitRegistrationApplication.DBModel;

namespace VisitRegistrationApplication.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            using (DentistDBEntities objDentistDBEntities = new DentistDBEntities())
            {
                return View();
            }
        }
    }
}