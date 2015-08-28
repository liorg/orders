using Michal.Project.Models;
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
            //string path = HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data/") + "cars.xml";
            string path = "rr.xml";//HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data/") + "cars.xml";
            Streets streets;
            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            XmlSerializer mySerializer =
            new XmlSerializer(typeof(Streets));
            // To read the file, create a FileStream.
            FileStream myFileStream = new FileStream(path, FileMode.Open);
            // Call the Deserialize method and cast to the object type.
            streets = (Streets)mySerializer.Deserialize(myFileStream);
            var googleformat = "http://maps.googleapis.com/maps/api/geocode/json?address={0},+{1},+{2}&sensor=true_or_false";
            StreetsGeoLocation location = new StreetsGeoLocation();
            foreach (var street in streets.StreetsItems)
            {
                var streetLatAndLng = new StreetLatAndLng();
                streetLatAndLng.Addr = street.Addr;
                streetLatAndLng.City = street.City;
                streetLatAndLng.CodeAddr = street.CodeAddr;
                streetLatAndLng.CodeCity = street.CodeCity;
                streetLatAndLng.Id = street.Id;
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
                    var result=o.results;
                    var status = o.status;
                    if (status.Value == "ZERO_RESULTS") 
                        continue;
                }
                catch (Exception ee)
                {
                    
                    //throw;
                }

                //http://maps.googleapis.com/maps/api/geocode/json?address=1600+Amphitheatre+Parkway,+Mountain+View,+CA&sensor=true_or_false
                //http://maps.googleapis.com/maps/api/geocode/json?address=אבולפיה,+תל אביב - יפו,+ישראל&sensor=true_or_false
               
            }

            //var st = new Streets();
            //st.StreetsItems = new List<Street>();
            //st.StreetsItems.Add(new Street { Addr = "1", City = "2", CodeAddr = "3", CodeCity = "4", Id = "5" });

            //st.StreetsItems.Add(new Street { Addr = "1", City = "2", CodeAddr = "3", CodeCity = "4", Id = "5" });
            //XmlSerializer s = new XmlSerializer(st.GetType());
            //StringBuilder sb = new StringBuilder();
            //TextWriter w = new StringWriter(sb);
            //s.Serialize(w, st);
            //w.Flush();

        }
    }
}
