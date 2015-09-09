﻿using Michal.Project.Dal;
using Michal.Project.Models;
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
            var cities = mem.GetCities();
            var search = cities.Where(c => c.Value.Contains(term)).ToList();
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

    }
}