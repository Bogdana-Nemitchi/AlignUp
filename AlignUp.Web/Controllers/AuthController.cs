using AlignUp.BusinessLogic.Interface;
using AlignUp.Domain.Model.User;
using AlignUp.Web.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

//namespace AlignUp.Web.Controllers
//{
   // public class AuthController : Controller
    //{
      //  private readonly IAuth _auth;
        //public AuthController()
        //{
         //   var bl = new BusinessLogic.BusinessLogic();
          //  _auth = bl.GetAuthBL();
        //}

        //[HttpGet]
        //public ActionResult Login()
        //{
          //  return View("Login", new UserLoginDTO());
        //}

        //[HttpPost]
        //public ActionResult Auth(UserDataLogin loginForm)
        //{
          //  if (!ModelState.IsValid)
            //{
              //  ViewBag.Error = "Datele introduse nu sunt valide.";
                //return View("Login", new UserLoginDTO { Username = loginForm.Username });
            //}

            //var loginDataForLogic = new UserLoginDTO
            //{
              //  Password = loginForm.Password,
                //Username = loginForm.Username,
                //UserIp = Request.UserHostAddress
            //};
            
            //string token = _auth.UserAuthLogic(loginDataForLogic);

            //if (!string.IsNullOrEmpty(token))
            //{
              //  Session["UserToken"] = token;
               // Session["User"] = loginForm.Username;
                //Session["LoginTime"] = DateTime.UtcNow;
                
                //return RedirectToAction("Index", "Home");
            //}

            //ViewBag.Error = "Username sau parolă incorectă!";
            //return View("Login", new UserLoginDTO { Username = loginForm.Username });
        //}

//        [HttpGet]
  //      public ActionResult Logout()
    //    {
       
      // return View();
        //}

          //  if (Request.Cookies["ASP.NET_SessionId"] != null)
            //{
              //  Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            //}

          //  if (Request.Cookies[AntiForgeryConfig.CookieName] != null)
            //{
              //  Response.Cookies[AntiForgeryConfig.CookieName].Expires = DateTime.Now.AddDays(-1);
            //}

            //return RedirectToAction("Login", "Auth");
        //}
        
        //public ActionResult DevLogin(string username)
        //{
          //  if (!string.IsNullOrEmpty(username) &&
            //    (username.Equals("Andriana", StringComparison.OrdinalIgnoreCase) ||
              //   username.Equals("admin", StringComparison.OrdinalIgnoreCase)))
            //{
              //  Session["User"] = username;
               // Session["UserToken"] = "DEV_TOKEN_" + Guid.NewGuid().ToString();
                //Session["LoginTime"] = DateTime.UtcNow;
                
                //return RedirectToAction("Index", "Home");
            //}

            //return RedirectToAction("Login");
        //}
    //}
//}