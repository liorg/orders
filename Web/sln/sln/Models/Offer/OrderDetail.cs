using Michal.Project.Contract.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class OrderDetail : IShipView
    {
        [Display(Name = "מזהה משלוח")]
        public Guid Id { get; set; }
        [Display(Name = "מזהה הצעה")]
        public Guid OfferId { get; set; }
        [Display(Name = " חברת משלוחים")]
        public Guid ShippingCompanyId { get; set; }
        [Display(Name = "מספר משלוח")]
        public string Name { get; set; }

        [Display(Name = " כותרת")]
        public string Title { get; set; }

        public int StateCode { get; set; }
        public Guid StatusId { get; set; }

        [Display(Name = "כתובת יעד")]
        public AddressEditorViewModel TargetAddress { get; set; }

        [Display(Name = "כתובת מקור")]
        public AddressEditorViewModel SourceAddress { get; set; }

        public List<ShippingItemVm> ShippingItems { get; set; }

        [Display(Name = "כיוון משלוח")]
        public string DirectionText { get; set; }

        [Display(Name = "החזרת אסמכתא")]
        public string SigTypeText { get; set; }

        [Display(Name = "סוג משלוח")]
        public string ShipTypeText { get; set; }

        [Display(Name = "מרחק")]
        public string DistanceText { get; set; }
    }
}