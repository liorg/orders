using sln.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace sln.DataModel
{

    public class ShippingItem : IModifieder
    {
        public ShippingItem()
        {

        }

        public virtual Product Product { get; set; }
        [ForeignKey("Product")]
        public Guid? Product_ProductId { get; set; }

        public virtual Shipping Shipping { get; set; }
        [ForeignKey("Shipping")]
        public Guid? Shipping_ShippingId { get; set; }


        [Key]
        public Guid ShippingItemId { get; set; }

        public string Name { get; set; }

        public DateTime? CreatedOn
        {
            get;
            set;
        }

        public DateTime? ModifiedOn
        {
            get;
            set;
        }

        public Guid? CreatedBy
        {
            get;
            set;
        }

        public Guid? ModifiedBy
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public double Quantity { get; set; }

    }
}