using Michal.Project.Dal;
using Michal.Project.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        void CallGoogle(StreetLatAndLng streetLatAndLng, AddressEditorViewModel addr)
        {
            try
            {
                // Thread.Sleep(200);
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
                   // Console.WriteLine("{0} ,{1},City ={2},Addr={3} ", streetLatAndLng.Lat, streetLatAndLng.Lng, streetLatAndLng.City, streetLatAndLng.Addr);

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