using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Michal.Project
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles); //ggg
        }


        //http://blog.nvise.com/?p=26
        /// <summary>
        /// ASP.NET MVC returning 302 (Found) HTTP status code on unauthorized Ajax calls instead of 401(Unauthorized) like classic ASP.NET
        /// </summary>
        protected void Application_EndRequest()
        {
            if (Context.Response.StatusCode == 302 &&
                Context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                Context.Response.Clear();
                Context.Response.StatusCode = 401;
            }
        }
    }

}
