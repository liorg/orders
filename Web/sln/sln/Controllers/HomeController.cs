using Michal.Project.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Michal.Project.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Version()
        {
            ViewBag.Version = "1.0.0.0";
            return View();
        }  

       
    }
}