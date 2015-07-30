using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln.Models
{
    public class Runner
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string FullName
        {
            get
            { return FirstName + " " + Lastname; }
        }
    }
}