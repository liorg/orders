using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WhatsAppApi;
using WhatsAppApi.Account;

namespace Michal.Project.Api
{
    public class MessagesController : ApiController
    {
        // [Route("")]
        [AcceptVerbs("GET")]
        public IHttpActionResult Get(string s,string d)
        {
            var tmpEncoding = Encoding.UTF8;
            //System.Console.OutputEncoding = Encoding.Default;
          //  System.Console.InputEncoding = Encoding.Default;
            string nickname = "WhatsApiNet";
            string sender = System.Configuration.ConfigurationManager.AppSettings["wp_sender"].ToString(); // Mobile number with country code (but without + or 00)
            string password = System.Configuration.ConfigurationManager.AppSettings["wp_pws"].ToString(); //v2 password
            string target = d;//"972549411222";// Mobile number to send the message to
            Task.Run(() =>
            {
                WhatsApp wa = new WhatsApp(sender, password, nickname, true);
                //event bindings
                wa.OnLoginSuccess += (phoneNumber, data) =>
                {
                    WhatsUserManager usrMan = new WhatsUserManager();
                    var tmpUser = usrMan.CreateUser(target, "User");
                    string sdata = Convert.ToBase64String(data);
                    wa.SendMessage(tmpUser.GetFullJid(), s);
                };
                byte[] nextChallenge = Convert.FromBase64String(sender);
                wa.Connect();
                wa.Login(nextChallenge);
            });
            return Ok("test=" + s);
        }

        private void Wa_OnLoginSuccess(string phoneNumber, byte[] data)
        {
            //  wa.SendMessage(tmpUser.GetFullJid(), line);
        }
    }
}
