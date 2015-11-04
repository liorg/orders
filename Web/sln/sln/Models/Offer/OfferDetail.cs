using Michal.Project.Contract.View;
using Michal.Project.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class OfferDetail
    {
        public List<ItemPrice> Items { get; set; }
        public List<DiscountItem> DiscountItems { get; set; }

        public OfferItemVm OfferItemVm { get; set; }
    }

    public class OfferClient : IShipView
    {

        public Guid Id { get; set; }
        public Guid OfferId { get; set; }

        public string Name { get; set; }

        public List<OfferClientItem> Discounts { get; set; }
       // public List<OfferClientItem> AddItems { get; set; }
        public List<OfferClientItem> Distance { get; set; }
        public List<OfferClientItem> ShipType { get; set; }
        public List<OfferClientItem> Products { get; set; }

        public OfferClient()
        {

        }

        public bool AllowAddDiscount
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.User != null &&
                    (HttpContext.Current.User.IsInRole(HelperAutorize.RoleAdmin) || HttpContext.Current.User.IsInRole(HelperAutorize.RunnerManager)
                    || HttpContext.Current.User.IsInRole(HelperAutorize.RoleOrgManager)
                    ))
                    return true;
                return false;
            }
        }

        public bool AllowAddItem
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.User != null &&
                    (HttpContext.Current.User.IsInRole(HelperAutorize.RoleAdmin) || HttpContext.Current.User.IsInRole(HelperAutorize.RunnerManager)))
                    return true;
                return false;
            }
        }
        //public double? Total { get; set; }

        public List<OfferItem> Items { get; set; }
    }
    public class OfferClientItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public Guid StatusShipping { get; set; }
        public decimal? PriceValue
        {
            get
            {
                return ProductPrice.HasValue ? ProductPrice.Value * Amount : ProductPrice;
            }
        }
        public bool IsPresent { get; set; }
        public bool IsDiscount { get; set; }
        public int StatusRecord { get; set; } // 1 =addbysystem,2=remove,3=addnew,4=edit
        public int Amount { get; set; }
        public decimal? ProductPrice { get; set; }

        public bool HasPrice
        {
            get
            {
                return PriceValue.HasValue;
            }
        }
        public bool AllowEdit
        {
            get;
            set;
            //get
            //{
            //    if (HttpContext.Current != null && HttpContext.Current.User != null &&
            //        (HttpContext.Current.User.IsInRole(HelperAutorize.RoleAdmin) || HttpContext.Current.User.IsInRole(HelperAutorize.RunnerManager)))
            //        return true;
            //    return false;
            //}
        }
        public bool AllowRemove
        {
            get;
            set;
            //get
            //{
            //    if (HttpContext.Current != null && HttpContext.Current.User != null &&
            //        (HttpContext.Current.User.IsInRole(HelperAutorize.RoleAdmin) || HttpContext.Current.User.IsInRole(HelperAutorize.RunnerManager)))
            //        return true;
            //    return false;
            //}
        }
        public bool AllowCancel
        {
            get;
            set;
            //get
            //{
            //    if (HttpContext.Current != null && HttpContext.Current.User != null &&
            //        (HttpContext.Current.User.IsInRole(HelperAutorize.RoleAdmin) || HttpContext.Current.User.IsInRole(HelperAutorize.RunnerManager)))
            //        return true;
            //    return false;
            //}
        }
    }

    public class OfferItem : OfferClientItem
    {
        public Guid ObjectId { get; set; }
        public int ObjectIdType { get; set; }
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
                Name = "הנחה שניה",
                Desc = "הנחה שניה...",
                IsDiscount = true,
                IsPresent = true,
                ProductPrice = 1,
                StatusRecord = 1
            });
            Discounts.Add(new OfferClientItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Name = "הנחה שלישית",
                Desc = "הנחה שלישית...",
                IsDiscount = true,
                IsPresent = false,
                ProductPrice = 22,
                StatusRecord = 1
            });
            Discounts.Add(new OfferClientItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000023"),
                Name = "הנחה רביעית",
                Desc = "הנחה רביעית...",
                IsDiscount = true,
                IsPresent = false,
                ProductPrice = 1,
                StatusRecord = 1
            });
            AddItems = new List<OfferClientItem>();
            AddItems.Add(new OfferClientItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Name = "a1",
                IsDiscount = false,
                IsPresent = false,
                ProductPrice = 100,
                StatusRecord = 1
            });
            AddItems.Add(new OfferClientItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                Name = "a2",
                IsDiscount = false,
                IsPresent = false,
                ProductPrice = 100,
                StatusRecord = 1
            });
        }
    }


}