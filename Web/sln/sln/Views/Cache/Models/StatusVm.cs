using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class StatusVm
    {
        public Guid StatusId { get; set; }
        public string Name { get; set; }
        public int MessageType { get; set; }
        public string Message { get; set; }
        public Guid ShipId { get; set; }
        public string Recipient { get; set; }
        public List<Runner> Runners { get; set; }
    }
}