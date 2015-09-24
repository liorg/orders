using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class Location
    {
        public double TargetLat { get; set; }
        public double TargetLng { get; set; }
        public string TargetName { get; set; }

        public double SourceLat { get; set; }
        public double SourceLng { get; set; }
        public string SourceName { get; set; }

    }
}