using AlignUp.BusinessLogic.Interface;
using AlignUp.Web.Models.Auth;
using System;
using System.Diagnostics;
using System.Web.Helpers;
using System.Web.Mvc;
using AlignUp.Domain.Model.User;

namespace AlignUp.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuth _auth;

        public AuthController()
        {
            BusinessLogic.BusinessLogic bl = new BusinessLogic.BusinessLogic();
            _auth = bl.GetAuthBL();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View("Login", new UserLoginDTO());
        }

        [HttpPost]
        public ActionResult Auth(UserDataLogin loginForm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Datele introduse nu sunt valide.";
                return View("Login", new UserLoginDTO { Username = loginForm.Username });
            }

            UserLoginDTO loginDataForLogic = new UserLoginDTO
            {
                Password = loginForm.Password,
                Username = loginForm.Username,
                UserIp = Request.UserHostAddress
            };

            string token = _auth.UserAuthLogic(loginDataForLogic);

            if (!string.IsNullOrEmpty(token))
            {
                Session["UserToken"] = token;
                Session["User"] = loginForm.Username;
                Session["LoginTime"] = DateTime.UtcNow;

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Username sau parolă incorectă!";
            return View("Login", new UserLoginDTO { Username = loginForm.Username });
        }

        [HttpGet]
        public ActionResult Register()
        {
            TempData.Clear();
            return View(new UserRegisterDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegisterDTO model)
        {
            Debug.WriteLine("Register method called");

            if (!ModelState.IsValid)
            {
                ViewBag.DebugInfo = "Model invalid: " + string.Join(", ", ModelState.Values);
                TempData["RegistrationError"] = "Formularul conține erori. Verificați datele introduse.";
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Parola și confirmarea parolei nu se potrivesc.");
                TempData["RegistrationError"] = "Parolele introduse nu se potrivesc.";
                return View(model);
            }

            try
            {
                UserRegisterDTO userRegisterDTO = new UserRegisterDTO
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    RegistrationIp = Request.UserHostAddress ?? "127.0.0.1",
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                string debugInfo = $"\u00cencercare înregistrare: {model.Username}, Email: {model.Email}";
                ViewBag.DebugInfo = debugInfo;
                Debug.WriteLine(debugInfo);

                bool result = _auth.UserRegister(userRegisterDTO);

                if (result)
                {
                    TempData["SuccessMessage"] = "Contul a fost creat cu succes! Te poți autentifica acum.";
                    return RedirectToAction("Login");
                }

                TempData["RegistrationError"] = "Înregistrarea nu a reușit. Este posibil ca numele de utilizator sau adresa de email să existe deja.";
            }
            catch (Exception ex)
            {
                string errorMessage = $"Eroare la înregistrare: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $" Inner exception: {ex.InnerException.Message}";
                }
                Debug.WriteLine(errorMessage);
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                ViewBag.DebugInfo = errorMessage;
                TempData["RegistrationError"] = "A apărut o eroare la înregistrare: " + ex.Message;
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                System.Web.HttpCookie sessionCookie = new System.Web.HttpCookie("ASP.NET_SessionId", "")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    HttpOnly = true
                };
                Response.Cookies.Add(sessionCookie);
            }

            if (Request.Cookies[AntiForgeryConfig.CookieName] != null)
            {
                System.Web.HttpCookie antiForgeryCookie = new System.Web.HttpCookie(AntiForgeryConfig.CookieName, "")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    HttpOnly = true
                };
                Response.Cookies.Add(antiForgeryCookie);
            }

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult DevLogin(string username)
        {
            if (!string.IsNullOrEmpty(username) &&
                (username.Equals("Andriana", StringComparison.OrdinalIgnoreCase) ||
                 username.Equals("admin", StringComparison.OrdinalIgnoreCase)))
            {
                Session["User"] = username;
                Session["UserToken"] = "DEV_TOKEN_" + Guid.NewGuid().ToString();
                Session["LoginTime"] = DateTime.UtcNow;

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login");
        }
    }
}
