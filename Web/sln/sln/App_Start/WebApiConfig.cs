using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
//using System.Web.Http.Cors;
namespace Michal.Project
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.EnableCors(new EnableCorsAttribute("*", "*", "*") { SupportsCredentials = true });
            // New code
           

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
