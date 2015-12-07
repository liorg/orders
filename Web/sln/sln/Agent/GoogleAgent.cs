using Michal.Project.Contract.Agent;
using Michal.Project.Contract.DAL;
using Michal.Project.Dal;
using Michal.Project.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Agent
{
    public class GoogleAgent : ILocationAgent
    {
     //   ILocationRepository _locationRepository;
        public GoogleAgent()
        {
          
        }
        
        public async Task SetLocationAsync(ILocationRepository locationRepository,AddressEditorViewModel source, Michal.Project.DataModel.Address target)
        {
            if (IsChanged(source))
            {
                var streetlan = locationRepository.GetStreetsGeoLocation().StreetsItems.Where(st => st.CodeCity == source.Citycode && st.CodeAddr == source.Streetcode).FirstOrDefault();
                if (streetlan != null)
                    await CallGoogleAsync(streetlan, source);
            }
            Map(source, target);
        }

        async Task CallGoogleAsync(StreetLatAndLng streetLatAndLng, AddressEditorViewModel addr)
        {
            try
            {
                addr.UId = streetLatAndLng.UId;
                addr.Lng = streetLatAndLng.Lng;
                addr.Lat = streetLatAndLng.Lat;

                using (var httpClient = new HttpClient())
                {
                    var url = String.Format(streetLatAndLng.GoogleFromatApiUrl, addr.Num);
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        //will throw an exception if not successful
                        //response.EnsureSuccessStatusCode();
                        //Product product = await response.Content.ReadAsAsync>Product>();
                        var content = await response.Content.ReadAsStringAsync();
                        dynamic o = JObject.Parse(content);
                        var result = o.results;
                        if (result != null && result.Count > 0 && result[0] != null && result[0].geometry != null && result[0].geometry.location != null)
                        {
                            var loc = result[0].geometry.location;

                            addr.Lat = loc.lat;
                            addr.Lng = loc.lng;
                            addr.IsSensor = true;
                        }
                        var status = o.status;

                        streetLatAndLng.Status = status.Value;

                        if (status.Value == "ZERO_RESULTS")
                        {
                            Console.WriteLine("ZERO_RESULTS,City ={0},Addr={1} ", streetLatAndLng.City, streetLatAndLng.Addr);
                            //continue;
                        }
                        if (status.Value == "OVER_QUERY_LIMIT")
                        {
                            Console.WriteLine("OVER_QUERY_LIMIT,City ={0},Addr={1} ", streetLatAndLng.City, streetLatAndLng.Addr);

                            //continue;
                        }
                    }
                }

            }
            catch (Exception ee)
            {
            }
        }

        void Map(AddressEditorViewModel source, Michal.Project.DataModel.Address target)
        {
            target.StreetName = source.Street;
            target.CityName = source.City;
            target.CityCode = source.Citycode;
            target.StreetCode = source.Streetcode;
            target.Lat = source.Lat;
            target.Lng = source.Lng;
            target.UID = source.UId.GetValueOrDefault();
            target.IsSensor = source.IsSensor;
            target.StreetNum = source.Num;
        }

        bool IsChanged(AddressEditorViewModel addr)
        {
            if (addr.Citycode != addr.CitycodeOld || addr.Streetcode != addr.StreetcodeOld || addr.Num != addr.NumOld)
                return true;
            return false;

        }

        public async Task FindDistance(Michal.Project.DataModel.Address from, Michal.Project.DataModel.Address to)
        {
            string api = System.Configuration.ConfigurationManager.AppSettings["googleapi"];

            //https://maps.googleapis.com/maps/api/distancematrix/json?origins=32.0228952,34.7552509&destinations=32.013767,34.761126&key=AIzaSyC2HKhSRdOyPmV7lGMj0tdcfoaOY9XWi8Q
            var urlFormat = "https://maps.googleapis.com/maps/api/distancematrix/json?origins={0},{1}&destinations={2},{3}&key={4}";


            using (var httpClient = new HttpClient())
            {
                var url = String.Format(urlFormat, from.Lat, from.Lng, to.Lat, to.Lng, api);

            }
            /*
             * 
            {
   "destination_addresses" : [ "תל חי 9, בת ים, ישראל" ],
   "origin_addresses" : [ "תנין 4, בת ים, ישראל" ],
   "rows" : [
      {
         "elements" : [
            {
               "distance" : {
                  "text" : "1.5 ק\"מ",
                  "value" : 1510
               },
               "duration" : {
                  "text" : "5 דקות",
                  "value" : 318
               },
               "status" : "OK"
            }
         ]
      }
   ],
   "status" : "OK"
}
             */

            /*
             * {
   "destination_addresses" : [],
   "error_message" : "The provided API key is invalid.",
   "origin_addresses" : [],
   "rows" : [],
   "status" : "REQUEST_DENIED"
}
             **/
        }
    }
}