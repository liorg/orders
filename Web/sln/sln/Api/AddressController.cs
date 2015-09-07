using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Michal.Project.Api
{
    [RoutePrefix("api/Address")]
    public class AddressController : ApiController
    {
        [Route("Cities")]
        public IHttpActionResult GetCities()
        {
            //StreetsGeoLocation location = new StreetsGeoLocation();
            string path = System.Web.HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data/") + "rechovArrange.json";
               return Ok(new string[] { "value1", path });
        }

    }
}