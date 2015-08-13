using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class ViewType
    {
        public int Val { get; set; }
        public string Name { get; set; }
        
    }

    public class ViewOrder<T> 
    {
        public T Model { get; set; }
        public List<ViewType> ViewTypes { get; set; }
        
    }
   

    public class ShippingItemVm
    {
        [Display(Name = "מזהה")]
        public Guid Id { get; set; }

        [Display(Name = "מספר הזמנה")]
        public string OrderNumber { get; set; }

        [Display(Name = "שם")]
        public string Name { get; set; }

        [Display(Name = "מוצר")]
        public string ProductName { get; set; }

        [Display(Name = "מוצר")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage="ערך כמות חובה")]
        [Range(1, 1000,
        ErrorMessage = "הערך כמות  {0} חייב להיות בן {1} ו {2}.")]
        [Display(Name = "כמות")]
        public int Total { get; set; }

        [Display(Name = "תאריך יצירה ")]
        public string CreatedOn { get; set; }

        [Display(Name = "נוצר ע''י ")]
        public string CreatedBy { get; set; }

        [Display(Name = "פעיל")]
        public bool  IsActive { get; set; }

        [Display(Name = "משלוח")]
        public Guid ShipId { get; set; }


    }
}