using sln.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sln.Controllers
{
    public class HomeController : Controller
    {
        [LayoutInjecterAttribute("~/Views/Shared/Homepage.cshtml")]
        public ActionResult Index()
        {
            return View();
        }

       
    }
}