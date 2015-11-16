<%@ WebHandler Language="C#" Class="SendNotify" %>

using System;
using System.Web;
using System.Net;
using System.IO;
public class SendNotify : IHttpHandler
{
    // static string deviceid = "";
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
        context.Response.AppendHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS, HEAD");
        context.Response.AppendHeader("Access-Control-Allow-Headers", "X-Requested-With");
       
        string SERVER_API_KEY = "AIzaSyC2HKhSRdOyPmV7lGMj0tdcfoaOY9XWi8Q";
        var SENDER_ID = "530446261684"; 
        var value = "s";

        WebRequest tRequest;
        tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
        tRequest.Method = "post";
        tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
        tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));
        var deviceid = context.Request["d"].ToString();

        if (context.Request["b"] != null)
        {
            value = context.Request["b"];
        }
        tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
        string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.data=" + value + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + deviceid + "";
        //string postData = "data.data=" + value + "&registration_id=" + deviceid + "";
        
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