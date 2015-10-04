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
    public class LocationAgent
    {
        MemeryCacheDataService _memeryCacheDataService;
        public LocationAgent(MemeryCacheDataService memeryCacheDataService)
        {
            _memeryCacheDataService = memeryCacheDataService;
        }
        public void SetLocation(AddressEditorViewModel source, Michal.Project.DataModel.Address target)
        {
            if (IsChanged(source))
            {
                var streetlan = _memeryCacheDataService.GetStreetsGeoLocation().StreetsItems.Where(st => st.CodeCity == source.Citycode && st.CodeAddr == source.Streetcode).FirstOrDefault();
                if (streetlan != null)
                    CallGoogle(streetlan, source);
            }
            Map(source, target);
        }

        public async Task SetLocationAsync(AddressEditorViewModel source, Michal.Project.DataModel.Address target)
        {
            if (IsChanged(source))
            {
                var streetlan = _memeryCacheDataService.GetStreetsGeoLocation().StreetsItems.Where(st => st.CodeCity == source.Citycode && st.CodeAddr == source.Streetcode).FirstOrDefault();
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


        void CallGoogle(StreetLatAndLng streetLatAndLng, AddressEditorViewModel addr)
        {
            try
            {
                addr.UId = streetLatAndLng.UId;
                addr.Lng = streetLatAndLng.Lng;
                addr.Lat = streetLatAndLng.Lat;

                var httpClient = new HttpClient();
                var url = String.Format(streetLatAndLng.GoogleFromatApiUrl, addr.Num);
                var response = httpClient.GetAsync(url).Result;
                //will throw an exception if not successful
                response.EnsureSuccessStatusCode();

                string content = response.Content.ReadAsStringAsync().Result;
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
    }
}