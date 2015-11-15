<%@ WebHandler Language="C#" Class="getNotify" %>

using System;
using System.Web;
using System.Net;
using System.IO;
public class getNotify : IHttpHandler
{
   // static string deviceid = "";
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
        context.Response.AppendHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS, HEAD");
        context.Response.AppendHeader("Access-Control-Allow-Headers", "X-Requested-With");
       
        var deviceid = "";
        
            // var deviceid = "APA91bFdjYC0m0jBOqaQMlfH-8deBXWXopwAH9XXbntcl6wxsV2A5WVMv4gAx4yOJWhHs1s2KAQYZ3g1YKjAmFDs0hdukCpKR3EADWZLv99adSUIFW8lQfdJTrnt4vfZKU4TV0i3ToNHlYY1vdTGAU2_HlOWNMlcMjAReEN2ybCqxYZY2YJX10wchh7Oqmb3HwEz6UzQDr98";
            if (context.Request["d"] != null && !String.IsNullOrEmpty(context.Request["d"].ToString()))
            {
                deviceid = context.Request["d"].ToString(); //"APA91bEySCtvj53PBy2QgoxAgqlgfrIoN6cqQenbEn_2g7Sl_bSa_Uopp1eRe4m7VlleXJ4Jul5bj11Uz6oR1-3sN_XHDI00oNgebtFJlbqo5AjjxHgysegZ1jqsJIOmQja-akvxJ5GJlAJdaXgLl0FO9Dy_3EHVRLeQL0q2L-y5yvtxuQontlU";
            }

            var sResponseFromServer = "{\"query\":  {\"DeviceId\":\"" + deviceid + "\",\"Title\":\"hello\",\"Body\":\"go to url\",\"Url\":\"https://imaot.co.il\" } }";
        

        context.Response.Write(sResponseFromServer);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}