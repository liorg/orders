using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class OfferVm
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string StatusDesc{ get; set; }
        public string StatusReasonDesc { get; set; }
        public string ModifiedOnDesc { get; set; }
    }
}