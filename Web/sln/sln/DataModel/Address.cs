using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{
    [ComplexType]
    public class Address
    {
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string StreetCode { get; set; }
        public string StreetName { get; set; }
        public string ExtraDetail { get; set; }
        public string StreetNum{ get; set; }
        public bool IsSensor { get; set; }
        public int UID { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        public override string ToString()
        {
            return this.StreetName + " " + this.StreetNum + " ," + this.CityName + "(" + this.ExtraDetail + ")";
        }
    }
}