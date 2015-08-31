using Michal.Project.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XmlToJsonF4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.0.0.3 (4.0)");
            // Merge();
            var file = System.Configuration.ConfigurationSettings.AppSettings["file"].ToString();

            Init(file);
        }
        static void Init(string fileJosn)
        {
            StreetsGeoLocation location = new StreetsGeoLocation();
            location.StreetsItems = new List<StreetLatAndLng>();
            int idx = 0;
            using (StreamReader file = File.OpenText(fileJosn))

            //  using (StreamReader file = File.OpenText(@"rechovArrange2.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                StreetsGeoLocation locationDes = (StreetsGeoLocation)serializer.Deserialize(file, typeof(StreetsGeoLocation));
                var items = locationDes.StreetsItems.Where(w => String.IsNullOrEmpty(w.Status) || w.Status == "new" || w.Status == "OVER_QUERY_LIMIT").OrderBy(d => d.UId).ToList();
                var all = items.Count; bool stopService = false;
                foreach (var street in items)
                {

                    idx++;
                    var streetLatAndLng = new StreetLatAndLng();
                    streetLatAndLng.UId = street.UId;
                    streetLatAndLng.Addr = street.Addr;
                    streetLatAndLng.City = street.City;
                    streetLatAndLng.CodeAddr = street.CodeAddr;
                    streetLatAndLng.CodeCity = street.CodeCity;
                    streetLatAndLng.Id = street.Id;
                    streetLatAndLng.Tbl = street.Tbl;
                    streetLatAndLng.GoogleApiUrl = street.GoogleApiUrl;
                    streetLatAndLng.Lat = street.Lat;
                    streetLatAndLng.Lng = street.Lng;
                    streetLatAndLng.Status = street.Status;
                    if (streetLatAndLng.Lat == 0.0)
                    {
                        if (!stopService)
                        {
                            try
                            {
                                Thread.Sleep(200);
                               // var httpClient = new HttpClient();
                                //var response = httpClient.GetAsync(streetLatAndLng.GoogleApiUrl + "@client=AIzaSyC1gBDtccHjscCT6kVrIoNbF65YvB4j2o0").Result;
                                
                                //will throw an exception if not successful
                                //response.EnsureSuccessStatusCode();
                                var content = new System.Net.WebClient().DownloadString(streetLatAndLng.GoogleApiUrl + "@client=AIzaSyC1gBDtccHjscCT6kVrIoNbF65YvB4j2o0");
                               // string content = response.Content.ReadAsStringAsync().Result;
                                dynamic o = JObject.Parse(content);
                                var result = o.results;
                                if (result != null && result.Count > 0 && result[0] != null && result[0].geometry != null && result[0].geometry.location != null)
                                {
                                    var loc = result[0].geometry.location;

                                    streetLatAndLng.Lat = loc.lat;
                                    streetLatAndLng.Lng = loc.lng;
                                    Console.WriteLine("{0} ,{1},City ={2},Addr={3} ", streetLatAndLng.Lat, streetLatAndLng.Lng, streetLatAndLng.City, streetLatAndLng.Addr);

                                }
                                var status = o.status;

                                streetLatAndLng.Status = status.Value;

                                if (status.Value == "ZERO_RESULTS")
                                {
                                    Console.WriteLine("ZERO_RESULTS,City ={0},Addr={1} ", streetLatAndLng.City, streetLatAndLng.Addr);
                                    continue;
                                }
                                if (status.Value == "OVER_QUERY_LIMIT")
                                {
                                    Console.WriteLine("OVER_QUERY_LIMIT,City ={0},Addr={1} ", streetLatAndLng.City, streetLatAndLng.Addr);
                                    stopService = true;
                                    continue;
                                }

                            }
                            catch (Exception ee)
                            {
                            }
                        }
                    }
                    else
                    {
                        streetLatAndLng.Status = "Ok";
                    }
                    // streetLatAndLng.Status = "new";

                    //streetLatAndLng.Lat = street.Lat;
                    //streetLatAndLng.Lng = street.Lng;
                    Console.WriteLine("item {0} of {1}", idx, all);

                    location.StreetsItems.Add(streetLatAndLng);

                }
                using (FileStream fs = File.Open(@"rechovArrange" + DateTime.Now.ToString("yyyy-MM-dd hhmm") + ".json", FileMode.OpenOrCreate))
                using (StreamWriter sw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;

                    JsonSerializer seria = new JsonSerializer();
                    seria.Serialize(jw, location);
                }
            }

        }
    }
}
