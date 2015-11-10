using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Test
{
    /// <summary>
    /// Summary description for s
    /// </summary>
    public class s : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}