﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AlignUp.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            using (var db = new ApplicationDbContext())
            {
                db.SeedAdmin(); 
            }
        }

        void Session_Start(object sender, EventArgs e)
        {
            Session["User"] = null;
        }
    }
}
