using Michal.Project.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UTConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.0.0.3");
           // Merge();
            var file = System.Configuration.ConfigurationSettings.AppSettings["file"].ToString();

            Init(file);
            //
            //Test(file);
            //Test(@"rechovArrange2015-08-30 1127.json");
            //Test(@"rechovArrange2015-08-30 1228.json");
            //Test(@"rechovArrange2.json");
        }

        static void Test(string f)
        {
            StreetsGeoLocation location = new StreetsGeoLocation();
            location.StreetsItems = new List<StreetLatAndLng>();
          
            using (StreamReader file = File.OpenText(f))
            {
                JsonSerializer serializer = new JsonSerializer();
                StreetsGeoLocation locationDes = (StreetsGeoLocation)serializer.Deserialize(file, typeof(StreetsGeoLocation));
                var items = locationDes.StreetsItems.Where(w => String.IsNullOrEmpty(w.Status) || w.Status == "new" || w.Status == "OVER_QUERY_LIMIT").OrderBy(d => d.UId).ToList();
                var all = items.Count;
                Console.WriteLine("file={0}, count={1}",f, all);
            }

        }

        static void Merge()
        {
            StreetsGeoLocation location = new StreetsGeoLocation();
            location.StreetsItems = new List<StreetLatAndLng>();
            int idx = 0;
            using (StreamReader rechovArrange = File.OpenText(@"rechovArrange.json"))
            using (StreamReader file = File.OpenText(@"rechovArrange2015-08-30 1028.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                StreetsGeoLocation locationTarget = (StreetsGeoLocation)serializer.Deserialize(file, typeof(StreetsGeoLocation));
                StreetsGeoLocation locationSource = (StreetsGeoLocation)serializer.Deserialize(rechovArrange, typeof(StreetsGeoLocation));
                
                var itemsTarget = locationTarget.StreetsItems.OrderBy(d => d.Id).ToList();
                var itemsSource= locationSource.StreetsItems.OrderBy(d => d.Id).ToList();


                var all = itemsSource.Count;
                foreach (var streetSource in itemsSource)
                {
                    idx++;
                    var streetLatAndLng = new StreetLatAndLng();
                    streetLatAndLng.UId = idx;
                    streetLatAndLng.Addr = streetSource.Addr;
                    streetLatAndLng.City = streetSource.City;
                    streetLatAndLng.CodeAddr = streetSource.CodeAddr;
                    streetLatAndLng.CodeCity = streetSource.CodeCity;
                    streetLatAndLng.Id = streetSource.Id;
                    streetLatAndLng.Tbl = streetSource.Tbl;
                    streetLatAndLng.GoogleApiUrl = streetSource.GoogleApiUrl;

                    streetLatAndLng.Lat = streetSource.Lat;
                    streetLatAndLng.Lng = streetSource.Lng;
                    Console.WriteLine("item {0} of {1}", idx, all);


                    foreach (var streetTarget in itemsTarget)
                    {
                        if (streetTarget.UId == streetLatAndLng.UId)
                        {
                            streetLatAndLng.Lat = streetTarget.Lat;
                            streetLatAndLng.Lng = streetTarget.Lng;
                            streetLatAndLng.Status = streetTarget.Status;
                            break;
                        }
                    }
                    if (streetLatAndLng.Lat == 0.0 && String.IsNullOrEmpty(streetLatAndLng.Status))
                    {
                        streetLatAndLng.Status = "new";
                    }
                    else  if (streetLatAndLng.Lat != 0.0 && String.IsNullOrEmpty(streetLatAndLng.Status))
                    
                    {
                        streetLatAndLng.Status = "ok";
                    }
                   
                    location.StreetsItems.Add(streetLatAndLng);

                }
            }
            using (FileStream fs = File.Open(@"rechovArrange2.json", FileMode.OpenOrCreate))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, location);
            }


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
                var items = locationDes.StreetsItems.Where(w => String.IsNullOrEmpty( w.Status) ||  w.Status == "new" || w.Status == "OVER_QUERY_LIMIT").OrderBy(d => d.UId).ToList();
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
                                var httpClient = new HttpClient();
                                var response = httpClient.GetAsync(streetLatAndLng.GoogleApiUrl + "@client=AIzaSyC1gBDtccHjscCT6kVrIoNbF65YvB4j2o0").Result;
                                //will throw an exception if not successful
                                response.EnsureSuccessStatusCode();

                                string content = response.Content.ReadAsStringAsync().Result;
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

        static void Arrange()
        {
            StreetsGeoLocation location = new StreetsGeoLocation();
            location.StreetsItems = new List<StreetLatAndLng>();
            int idx = 0;
            using (StreamReader file = File.OpenText(@"rechov1.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                StreetsGeoLocation locationDes = (StreetsGeoLocation)serializer.Deserialize(file, typeof(StreetsGeoLocation));
                var items = locationDes.StreetsItems.OrderBy(d => d.Id).ToList();
                var all = items.Count;
                foreach (var street in items)
                {
                    idx++;
                    var streetLatAndLng = new StreetLatAndLng();
                    streetLatAndLng.UId = idx;
                    streetLatAndLng.Addr = street.Addr;
                    streetLatAndLng.City = street.City;
                    streetLatAndLng.CodeAddr = street.CodeAddr;
                    streetLatAndLng.CodeCity = street.CodeCity;
                    streetLatAndLng.Id = street.Id;
                    streetLatAndLng.Tbl = street.Tbl;
                    streetLatAndLng.GoogleApiUrl = street.GoogleApiUrl;
                    streetLatAndLng.Status = "new";
                    streetLatAndLng.Lat = street.Lat;
                    streetLatAndLng.Lng = street.Lng;
                    Console.WriteLine("item {0} of {1}", idx, all);

                    location.StreetsItems.Add(streetLatAndLng);
                }
            }
            using (FileStream fs = File.Open(@"rechovArrange.json", FileMode.OpenOrCreate))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, location);
            }

        }
        
        static void FirstLoad()
        {
            StreetsGeoLocation location = new StreetsGeoLocation();
            string path = "rr.xml";//HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data/") + "cars.xml";
            Streets streets = null;

            XmlSerializer mySerializer = new XmlSerializer(typeof(Streets));
            // To read the file, create a FileStream.
            using (FileStream myFileStream = new FileStream(path, FileMode.Open))
            {
                // Call the Deserialize method and cast to the object type.
                streets = (Streets)mySerializer.Deserialize(myFileStream);
            }

            //http://maps.googleapis.com/maps/api/geocode/json?address=1600+Amphitheatre+Parkway,+Mountain+View,+CA&sensor=true_or_false
            //http://maps.googleapis.com/maps/api/geocode/json?address=אבולפיה,+תל אביב - יפו,+ישראל&sensor=true_or_false

            var googleformat = "http://maps.googleapis.com/maps/api/geocode/json?address={0},+{1},+{2}&sensor=true_or_false";
            int idx = 0;
            int all = streets.StreetsItems.Count;
            location.StreetsItems = new List<StreetLatAndLng>();
            foreach (var street in streets.StreetsItems)
            {
                var streetLatAndLng = new StreetLatAndLng();
                streetLatAndLng.Addr = street.Addr;
                streetLatAndLng.City = street.City;
                streetLatAndLng.CodeAddr = street.CodeAddr;
                streetLatAndLng.CodeCity = street.CodeCity;
                streetLatAndLng.Id = street.Id;
                streetLatAndLng.Tbl = street.Tbl;
                //2247
                try
                {
                    var uri = String.Format(googleformat, street.Addr, street.City, "ישראל");
                    streetLatAndLng.GoogleApiUrl = uri;

                    var httpClient = new HttpClient();
                    var response = httpClient.GetAsync(uri).Result;

                    //will throw an exception if not successful
                    response.EnsureSuccessStatusCode();

                    string content = response.Content.ReadAsStringAsync().Result;
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
                    if (status.Value == "ZERO_RESULTS")
                    {
                        Console.WriteLine("no found,City ={0},Addr={1} ", streetLatAndLng.City, streetLatAndLng.Addr);
                        continue;
                    }

                }
                catch (Exception ee)
                {
                }
                Console.WriteLine("item {0} of {1}", idx, all);
                idx++;
                location.StreetsItems.Add(streetLatAndLng);
            }
            using (FileStream fs = File.Open(@"rechov1.json", FileMode.OpenOrCreate))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, location);
            }
        }
    }
}