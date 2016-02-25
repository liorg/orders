using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Michal.Project.logs
{
    /// <summary>
    /// Summary description for MobileLog
    /// </summary>
    public class MobileLog : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            StringBuilder err = new StringBuilder();
            var path = context.Server.MapPath("\\logs");
            var fileLog = Path.Combine(path, "error_" + DateTime.Now.Ticks + ".txt");
            foreach (string name in context.Request.Form)
            {
                err.AppendLine(name + ": " + context.Request.Form[name]);
            }

            TextWriter tw = null;
          
           
            try
            {
                tw = new StreamWriter(fileLog);
                tw.WriteLine(err.ToString());
            }
            catch (Exception) { }
            finally
            {
                if (tw != null)
                    tw.Close();
            }

            context.Response.Write("ok");

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