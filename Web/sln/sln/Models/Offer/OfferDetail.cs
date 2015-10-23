using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class OfferDetail
    {
        public List<ItemPrice> Items{ get; set; }
        public List<DiscountItem> DiscountItems { get; set; }
        
        public OfferItemVm OfferItemVm { get; set; }
    }

    public class OfferClient
    {
        public List<OfferClientItem> Discounts { get; set; }
        public List<OfferClientItem> AddItems { get; set; }
        public OfferClient()
        {
            
        }
        public double? Total { get; set; }

        public List<OfferClientItem> Items { get; set; }
    }

    public class OfferClientItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double? PriceValue { get; set; }
        public bool IsPresent { get; set; }
        public bool  IsDiscount { get; set; }
        public int StatusRecord  { get; set; } //1 =edit,2=remove,3=add
    }

    public class OfferDemo
    {
        public List<OfferClientItem> Discounts { get; set; }
        public List<OfferClientItem> AddItems { get; set; }

        public OfferDemo()
        {
            Discounts = new List<OfferClientItem>();
            Discounts.Add(new OfferClientItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "d1",
                IsDiscount = true,
                IsPresent = false,
                PriceValue = 1,
                StatusRecord = 1
            });
            Discounts.Add(new OfferClientItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Name = "d2",
                IsDiscount = true,
                IsPresent = false,
                PriceValue = 2,
                StatusRecord = 1
            });
            AddItems = new List<OfferClientItem>();
            AddItems.Add(new OfferClientItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Name = "a1",
                IsDiscount = false,
                IsPresent = false,
                PriceValue = 100,
                StatusRecord = 1
            });
            AddItems.Add(new OfferClientItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                Name = "a2",
                IsDiscount = false,
                IsPresent = false,
                PriceValue = 100,
                StatusRecord = 1
            });
        }
    }


}