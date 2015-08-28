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

    }
}