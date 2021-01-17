using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VisitRegistrationApplication.DBModel;
using VisitRegistrationApplication.Models;

namespace VisitRegistrationApplication.Controllers
{
    public class AccountController : Controller
    {
        DentistDBEntities3 objDentistDBEntities = new DentistDBEntities3();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            UserModel objUserModel = new UserModel();
            return View( objUserModel );
        }

        [HttpPost]
        public ActionResult Register(UserModel objUserModel)
        {
            if (ModelState.IsValid)
            {

                var objUser = new DBModel.Users();

                if(objDentistDBEntities.Users.Any(x =>x.Email == objUserModel.Email))
                {
                    ViewBag.ErrorEmailMessage = "Email już istnieje!";
                    return View("Register");
                }

                objUser.CreatedOn = DateTime.Now;
                objUser.FirstName = objUserModel.FirstName;
                objUser.LastName = objUserModel.LastName;
                objUser.Email = objUserModel.Email;
                objUser.Password = Hash.HashPassword(objUserModel.Password);
                objUser.Role = 2;

                objDentistDBEntities.Users.Add(objUser);
                objDentistDBEntities.SaveChanges();


                ViewBag.SuccessMessage = "Konto zostało utworzone!";
            }
            return View("Register");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel objLoginModel, string ReturnUrl)
        {
            string success = "";
            string error = "";

            var email = objDentistDBEntities.Users.Where(a => a.Email == objLoginModel.Email).FirstOrDefault();
            if (email != null)
            {
                if (string.Compare(Hash.HashPassword(objLoginModel.Password), email.Password) == 0)
                {
                    success = "Udało się! Jesteś zalogowany";
                    int timeout = 525600;
                    var ticket = new FormsAuthenticationTicket(objLoginModel.Email, true, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMilliseconds(timeout);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    } else
                    {
                        return RedirectToAction("Index", "User");
                    }

                } else
                {
                    error = "Zły hasło";
                }

            } else
            {
                error = "Zły email";
            }

            ViewBag.SuccessMessage = success;
            ViewBag.ErrorEmailMessage = error;

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}