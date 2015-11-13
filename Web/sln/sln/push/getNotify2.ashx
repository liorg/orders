<%@ WebHandler Language="C#" Class="getNotify2" %>

using System;
using System.Web;
using System.Net;
using System.IO;
public class getNotify2 : IHttpHandler
{
    // static string deviceid = "";
    public void ProcessRequest(HttpContext context)
    {
        var result = "";
        var url = "5.100.251.87";// "localhost";// "5.100.251.87";
        var deviceid = "";
        deviceid = context.Request["d"].ToString(); //"APA91bEySCtvj53PBy2QgoxAgqlgfrIoN6cqQenbEn_2g7Sl_bSa_Uopp1eRe4m7VlleXJ4Jul5bj11Uz6oR1-3sN_XHDI00oNgebtFJlbqo5AjjxHgysegZ1jqsJIOmQja-akvxJ5GJlAJdaXgLl0FO9Dy_3EHVRLeQL0q2L-y5yvtxuQontlU";

        context.Response.ContentType = "application/json";
        context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
        context.Response.AppendHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS, HEAD");
        context.Response.AppendHeader("Access-Control-Allow-Headers", "X-Requested-With");
        try
        {
            WebRequest tRequest;
            tRequest = WebRequest.Create("http://" + url + ":4545/api/Notifcation/GetNotify?deviceid=" + deviceid);
            tRequest.Method = "get";
            tRequest.ContentType = "application/json;charset=UTF-8";

            WebResponse tResponse = tRequest.GetResponse();
            var dataStream = tResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);
            String sResponseFromServer = tReader.ReadToEnd();
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(sResponseFromServer);
            if (data.IsError.Value)
            {
                result = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    IsError = true,
                    ErrDesc = data.ErrDesc.Value
                });
            }
            else
            {
                result = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    IsError = false,
                    ErrDesc = "s",
                    Title = data.Model.Title.Value,
                    Body = data.Model.Body.Value,
                    Url = data.Model.Url.Value
                });
            }
            tReader.Close();
            dataStream.Close();
            tResponse.Close();
        }
        catch (Exception e)
        {

            result = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                IsError = true,
                ErrDesc = e.ToString()
            });
        }

        context.Response.Write(result);
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}