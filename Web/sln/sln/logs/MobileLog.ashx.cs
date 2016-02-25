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
            try
            {
                StringBuilder err = new StringBuilder();
                var path = context.Server.MapPath("\\CollectLogs");
                var fileLog = Path.Combine(path, "android_" + DateTime.Now.Ticks + ".txt");
                foreach (string name in context.Request.Form)
                {
                    err.AppendLine(name + ": " + context.Request.Form[name]);
                }
                string json = new StreamReader(context.Request.InputStream).ReadToEnd();
                err.Append(json);
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


            }
            catch (Exception e)
            {

                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
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