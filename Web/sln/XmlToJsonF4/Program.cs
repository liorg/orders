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


using System.Web;
using System.Xml.Serialization;


namespace XmlToJsonF4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.0.0.5 framework 4");
            // Merge();
            var file = System.Configuration.ConfigurationSettings.AppSettings["file"].ToString();

            Init(file);
        }
        static void Distance()
        {
            // https://maps.googleapis.com/maps/api/distancematrix/json?origins=32.0228952,34.7552509&destinations=32.013767,34.761126&key=AIzaSyC2HKhSRdOyPmV7lGMj0tdcfoaOY9XWi8Q
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
             //   var items = locationDes.StreetsItems.Where(w => String.IsNullOrEmpty(w.Status) || w.Status == "new" || w.Status == "OVER_QUERY_LIMIT").OrderBy(d => d.UId).ToList();
                  var items = locationDes.StreetsItems.OrderBy(i => i.UId).ToList();//.Where(w => String.IsNullOrEmpty(w.Status) || w.Status == "new" || w.Status == "OVER_QUERY_LIMIT").OrderBy(d => d.UId).ToList();
               
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
                    streetLatAndLng.GoogleFromatApiUrl = street.GoogleFromatApiUrl;
                    streetLatAndLng.Lat = street.Lat;
                    streetLatAndLng.Lng = street.Lng;
                    streetLatAndLng.Status = street.Status;
                    if (streetLatAndLng.Lat == 0.0)
                    {
                       // if (!stopService)
                        //{
                        if (!stopService && (street.Status == "new" || street.Status == "OVER_QUERY_LIMIT"))
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
                                    location.StreetsItems.Add(streetLatAndLng);
                                    continue;
                                }
                                if (status.Value == "OVER_QUERY_LIMIT")
                                {
                                    Console.WriteLine("OVER_QUERY_LIMIT,City ={0},Addr={1} ", streetLatAndLng.City, streetLatAndLng.Addr);
                                    stopService = true;
                                    location.StreetsItems.Add(streetLatAndLng);
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
                using (FileStream fs = File.Open(@"rechovArrange" + DateTime.Now.ToString("yyyy-MM-dd HHmm") + ".json", FileMode.OpenOrCreate))
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

namespace Michal.Project.Models
{
    [XmlRoot("ROWDATA")]
    //  [XmlElement("ROWDATA")]
    public class Streets
    {
        //[XmlArray("ROW")]
        //[XmlArrayItem("ROW")]
        [XmlElement("ROW")]
        public List<Street> StreetsItems { get; set; }
    }

    //[XmlElement("ROW")]
    public class Street
    {
        [XmlElement("C0")]
        public string Id { get; set; }
        [XmlElement("סמל_ישוב")]
        public string CodeCity { get; set; }
        [XmlElement("שם_ישוב")]
        public string City { get; set; }

        [XmlElement("סמל_רחוב")]
        public string CodeAddr { get; set; }
        [XmlElement("שם_רחוב")]
        public string Addr { get; set; }
        [XmlElement("טבלה")]
        public string Tbl { get; set; }

    }
    [JsonObject]
    public class StreetsGeoLocation
    {

        public List<StreetLatAndLng> StreetsItems { get; set; }
    }
    public class StreetLatAndLng : Street
    {
        public string GoogleApiUrl { get; set; } 
        public string GoogleFromatApiUrl { get; set; }
        [XmlElement("Lat")]
        public double Lat { get; set; }
        [XmlElement("Lng")]
        public double Lng { get; set; }
        public string Desc { get; set; }
        public int UId { get; set; }
        public string Status { get; set; }//OVER_QUERY_LIMIT,ZERO_RESULTS
    }
}