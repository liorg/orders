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

        public int StateCode { get; set; } //1, new,2=checkin,3=checkout , 3=approval,7=exception
        public Guid StatusId { get; set; }

        public Guid ShippingCompanyId { get; set; }

        public bool IsDemo { get; set; }

        public bool AddExceptionPrice { get; set; }

        public Guid ObjectIdExcpetionPriceId { get; set; }

        public double? MaxPriceForGrantException { get; set; }

        public decimal? Total { get; set; }
        public decimal? Price { get; set; }
        public decimal? DiscountPrice { get; set; }

        public decimal? ClosedTotal { get; set; }
        public decimal? ClosedPrice { get; set; }
        public decimal? ClosedDiscountPrice { get; set; }

        public bool  IsClosed { get; set; }

    }

}