using Michal.Project.Dal;
using Michal.Project.Models;
using Michal.Project.Models.NSStreet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Michal.Project.Api
{
    [RoutePrefix("api/Ping")]
    public class PingController: ApiController
    {
        [Route("")]
        //[AcceptVerbs("GET")]
        public IHttpActionResult Get(string s)
        {
            return Ok("test="+s);
        }
        [Route("")]
       // [AcceptVerbs("GET")]
        public IHttpActionResult Get()
        {
            return Ok("test");
        }
        [Route("")]
       [AcceptVerbs("Post")]
        public IHttpActionResult Post(string s)
        {
            return Ok("test=" + s);
        }
    }
}