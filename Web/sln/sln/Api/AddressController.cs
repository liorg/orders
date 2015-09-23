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
   
    [RoutePrefix("api/Address")]
    public class AddressController : ApiController
    {
        [Route("Cities")]
        public HttpResponseMessage GetCities()
        {
            MemeryCacheDataService mem = new MemeryCacheDataService();
            var cities = mem.GetCities();
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<IEnumerable<KeyValuePairUI>>(cities,
                           new JsonMediaTypeFormatter(),
                            new MediaTypeWithQualityHeaderValue("application/json"))
            };
            //response.Headers.CacheControl = new CacheControlHeaderValue();
            //response.Headers.CacheControl.NoStore = true;
            return response;
        }
        [AcceptVerbs("GET")]
        [Route("SearchCities")]
        public HttpResponseMessage SearchCities(string term)
        {

            MemeryCacheDataService mem = new MemeryCacheDataService();
            var search = mem.GetCitiesByName(term.Trim());
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<IEnumerable<KeyValuePairUI>>(search,
                           new JsonMediaTypeFormatter(),
                            new MediaTypeWithQualityHeaderValue("application/json"))
            };
            //response.Headers.CacheControl = new CacheControlHeaderValue();
            //response.Headers.CacheControl.NoStore = true;
            return response;
        }
        [AcceptVerbs("GET")]
        [Route("SearchStreets")]
        public HttpResponseMessage SearchStreet([FromUri]StreetRequest req)

      // public HttpResponseMessage SearchStreet(int MaxItems, string ParentFilterId, string Term)
        {

            MemeryCacheDataService mem = new MemeryCacheDataService();
            //  var streets = mem.GetStreetByCityCode(ParentFilterId.Trim(), Term.Trim(), MaxItems);
            var streets = mem.GetStreetByCityCode(req.ParentFilterId.Trim(), req.Term.Trim(), req.MaxItems);

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<IEnumerable<KeyValuePairUI>>(streets,
                           new JsonMediaTypeFormatter(),
                            new MediaTypeWithQualityHeaderValue("application/json"))
            };
            //response.Headers.CacheControl = new CacheControlHeaderValue();
            //response.Headers.CacheControl.NoStore = true;
            return response;
        }
    }
}