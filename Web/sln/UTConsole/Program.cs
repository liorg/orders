using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            XmlSerializer serializer = new XmlSerializer(typeof(Streets));

            StreamReader reader = new StreamReader(path);
            reader.ReadToEnd();
            var strr = (Streets)serializer.Deserialize(reader);
            reader.Close();

           
            var st = new Streets();
            st.StreetsItems = new List<Street>();
            st.StreetsItems.Add(new Street { Addr = "1", City = "2", CodeAddr = "3", CodeCity = "4", Id = "5" });

            st.StreetsItems.Add(new Street { Addr = "1", City = "2", CodeAddr = "3", CodeCity = "4", Id = "5" });
            XmlSerializer s= new XmlSerializer(st.GetType());
            StringBuilder sb = new StringBuilder();
            TextWriter w = new StringWriter(sb);
            s.Serialize(w,st);
            w.Flush();
          
        }
    }
}
