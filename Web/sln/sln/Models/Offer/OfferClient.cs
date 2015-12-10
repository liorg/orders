using Michal.Project.Contract.View;
using Michal.Project.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{

    public class OfferClient : Offer, IView
    {
        public List<OfferClientItem> Discounts { get; set; }
        public List<OfferClientItem> DirtyDiscounts { get; set; }
        public List<OfferClientItem> Distances { get; set; }
        public List<OfferClientItem> ShipTypes { get; set; }
        public List<OfferClientItem> Products { get; set; }
        public List<OfferClientItem> BackTypes { get; set; }

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

        public Guid TimeWaitSetProductId { get; set; }

        public Guid TimeWaitGetProductId { get; set; }

        public double TimeWaitSend { get; set; }
        public double TimeWaitGet { get; set; }

        
    }
}