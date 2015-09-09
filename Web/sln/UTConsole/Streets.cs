using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

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
        public string GoogleFromatApiUrl { get; set; }
        public string GoogleApiUrl { get; set; }
        [XmlElement("Lat")]
        public double Lat { get; set; }
        [XmlElement("Lng")]
        public double Lng { get; set; }
        public string Desc { get; set; }
        public int UId { get; set; }
        public string Status { get; set; }//OVER_QUERY_LIMIT,ZERO_RESULTS
    }
}