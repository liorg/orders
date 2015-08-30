using Michal.Project.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UTConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Init();

        }
        static void Init()
        {
            StreetsGeoLocation location = new StreetsGeoLocation();
            location.StreetsItems = new List<StreetLatAndLng>();
            int idx = 0;
            using (StreamReader file = File.OpenText(@"rechovArrange.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                StreetsGeoLocation locationDes = (StreetsGeoLocation)serializer.Deserialize(file, typeof(StreetsGeoLocation));
                var items = locationDes.StreetsItems.Where(w => w.Status == "new" || w.Status == "OVER_QUERY_LIMIT").OrderBy(d => d.Id).ToList();
                var all = items.Count;
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
                    try
                    {


                        var httpClient = new HttpClient();
                        var response = httpClient.GetAsync(streetLatAndLng.GoogleApiUrl).Result;

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
                            Console.WriteLine("no found,City ={0},Addr={1} ", streetLatAndLng.City, streetLatAndLng.Addr);
                            continue;
                        }

                    }
                    catch (Exception ee)
                    {
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