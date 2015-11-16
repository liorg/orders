using Michal.Project.Contract.View;
using Michal.Project.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class Offer : IShipView
    {
        public Guid Id { get; set; }
        public Guid OfferId { get; set; }
        public string Name { get; set; }
        public bool HasDirty { get; set; }

        public bool StateCode { get; set; } //1, new,2=checkin,3=checkout , 3=approval
        public Guid StatusId { get; set; }

        public Guid ShippingCompanyId { get; set; }
       



    }

}