using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models.NSStreet
{
    public class StreetRequest
    {
        public int MaxItems { get; set; }
        public string ParentFilterId { get; set; }
        public string Term { get; set; }
    }
}