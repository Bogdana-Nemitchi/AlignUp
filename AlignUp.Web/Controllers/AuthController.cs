﻿using AlignUp.Domain.Interface;
using AlignUp.Domain.Model.User;
using AlignUp.Web.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using static AlignUp.Domain.Core.UserApi;
using UserRegisterDTO = AlignUp.Domain.Core.UserApi.UserRegisterDTO;

namespace AlignUp.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuth _auth;
        public AuthController()
        {
            var bl = new Domain.BusinessLogic();
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

            var loginDataForLogic = new UserLoginDTO
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
        public ActionResult Logout()
        {
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            }

            if (Request.Cookies[AntiForgeryConfig.CookieName] != null)
            {
                Response.Cookies[AntiForgeryConfig.CookieName].Expires = DateTime.Now.AddDays(-1);
            }

            Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View(new Domain.Model.User.UserRegisterDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AlignUp.Domain.Model.User.UserRegisterDTO registerData)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Datele introduse nu sunt valide.";
                return View(registerData);
            }

            registerData.RegistrationIp = Request.UserHostAddress;

            // Mapezi manual în tipul din business logic
            var mappedData = new AlignUp.Domain.Core.UserApi.UserRegisterDTO
            {
                Username = registerData.Username,
                Email = registerData.Email,
                Password = registerData.Password,
                RegistrationIp = registerData.RegistrationIp
            };

            var result = _auth.UserRegister(mappedData);
            if (result)
            {
                TempData["Success"] = "Contul a fost creat cu succes. Te poți autentifica.";
                return RedirectToAction("Login");
            }

            ViewBag.Error = "A apărut o eroare la înregistrare.";
            return View(registerData);
        }



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