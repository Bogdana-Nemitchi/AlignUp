using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AlignUp.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Код, выполняемый при запуске приложения
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Inițializează sesiunea pentru utilizator
            Session["User"] = null;
        }
    }
}