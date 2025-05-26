using System.Web.Mvc;

namespace AlignUp.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        // ✅ Metodă de test
        public ActionResult Test()
        {
            return Content("✔️ Funcționează - răspuns din metoda Test din HomeController");
        }
    }
}
