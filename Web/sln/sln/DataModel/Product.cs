using sln.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace sln.DataModel
{

    public class Product : IModifieder
    {
        public Product()
        {
            PriceLists = new HashSet<PriceList>();
            ShippingItems = new HashSet<ShippingItem>();
        }
        
        public ICollection<PriceList> PriceLists { get; set; }
        public ICollection<ShippingItem> ShippingItems { get; set; }

        // many to many
        public ICollection<Organization> Organizations { get; set; }
        [Key]
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string ProductNumber { get; set; }

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

        public bool IsCalculatingShippingInclusive { get; set; }


    }
}