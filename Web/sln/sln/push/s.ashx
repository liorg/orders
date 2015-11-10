<%@ WebHandler Language="C#" Class="s" %>

using System;
using System.Web;
using System.Net;
using System.IO;
public class s : IHttpHandler
{
   // static string deviceid = "";
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
        context.Response.AppendHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS, HEAD");
        context.Response.AppendHeader("Access-Control-Allow-Headers", "X-Requested-With");
        /*
         * curl --header "Authorization: key=AIzaSyBBh4ddPa96rQQNxqiq_qQj7sq1JdsNQUQ" --header Content-Type:"application/json" 
         * https://android.googleapis.com/gcm/send -d "{\"registration_ids\":[\"APA91bFdjYC0m0jBOqaQMlfH-8deBXWXopwAH9XXbntcl6wxsV2A5WVMv4gAx4yOJWhHs1s2KAQYZ3g1YKjAmFDs0hdukCpKR3EADWZLv99adSUIFW8lQfdJTrnt4vfZKU4TV0i3ToNHlYY1vdTGAU2_HlOWNMlcMjAReEN2ybCqxYZY2YJX10wchh7Oqmb3HwEz6UzQDr98\"]}"
         */
        string SERVER_API_KEY = "AIzaSyC2HKhSRdOyPmV7lGMj0tdcfoaOY9XWi8Q";//"AIzaSyDN5IIadRYEhkXYfL-velSE46dRHQ5wmI0"; //"AIzaSyBBh4ddPa96rQQNxqiq_qQj7sq1JdsNQUQ";
        var SENDER_ID = "530446261684"; //"530446261684";// "application number";

        var value = "s";
        
        WebRequest tRequest;
        tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
        tRequest.Method = "post";
        tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
        tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));
        var deviceid = "";
        if (deviceid == "")
        {
            // var deviceid = "APA91bFdjYC0m0jBOqaQMlfH-8deBXWXopwAH9XXbntcl6wxsV2A5WVMv4gAx4yOJWhHs1s2KAQYZ3g1YKjAmFDs0hdukCpKR3EADWZLv99adSUIFW8lQfdJTrnt4vfZKU4TV0i3ToNHlYY1vdTGAU2_HlOWNMlcMjAReEN2ybCqxYZY2YJX10wchh7Oqmb3HwEz6UzQDr98";
            if (context.Request["d"] != null && !String.IsNullOrEmpty(context.Request["d"].ToString()))
            {
                deviceid = context.Request["d"].ToString(); //"APA91bEySCtvj53PBy2QgoxAgqlgfrIoN6cqQenbEn_2g7Sl_bSa_Uopp1eRe4m7VlleXJ4Jul5bj11Uz6oR1-3sN_XHDI00oNgebtFJlbqo5AjjxHgysegZ1jqsJIOmQja-akvxJ5GJlAJdaXgLl0FO9Dy_3EHVRLeQL0q2L-y5yvtxuQontlU";
            }
        }
        
        tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
        //string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.data=" + value + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + deviceid + "";
        string postData = "data.data=" + value + "&registration_id=" + deviceid + "";
        Console.WriteLine(postData);
        
        Byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(postData);
        tRequest.ContentLength = byteArray.Length;

        Stream dataStream = tRequest.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        WebResponse tResponse = tRequest.GetResponse();

        dataStream = tResponse.GetResponseStream();

        StreamReader tReader = new StreamReader(dataStream);

        String sResponseFromServer = tReader.ReadToEnd();


        tReader.Close();
        dataStream.Close();
        tResponse.Close();

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