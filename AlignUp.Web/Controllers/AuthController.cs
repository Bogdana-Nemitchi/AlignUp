using AlignUp.BusinessLogic.Interface;
using AlignUp.Domain.Model.User;
using AlignUp.Web.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlignUp.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuth _auth;
        public AuthController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _auth = bl.GetAuthBL();
        }


        // GET: Auth1
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Auth(UserDataLogin login)
        {
            var data = new UserLoginDTO
            {
                Password = login.Password,
                Username = login.Username,
                UserIp = "localhost"
            };

            string token = _auth.UserAuthLogic(data);

            if (!string.IsNullOrEmpty(token))
            {
                Session["User"] = login.Username;
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Username sau parolă incorectă!";

            // Adaugă această linie pentru debugging:
            return View("Login", new UserLoginDTO());
        }


        public ActionResult Login()
        {
            return View("Login");
        }


        public ActionResult Logout()
        {
            Session.Clear(); // Șterge toate datele din sesiune
            return RedirectToAction("Index", "Home"); // Redirecționează utilizatorul la pagina principală
        }


    }
}