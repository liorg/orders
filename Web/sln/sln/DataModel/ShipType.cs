using sln.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace sln.DataModel
{

    public class ShipType : IModifieder
    {
        public ShipType()
        {
           Shippings = new HashSet<Shipping>();
           PriceLists = new HashSet<PriceList>();
           Discounts = new HashSet<Discount>();
        }
        
        public ICollection<PriceList> PriceLists { get; set; }
       
        public ICollection<Shipping> Shippings { get; set; }

        public ICollection<Discount> Discounts { get; set; }

        [Key]
        public Guid ShipTypeId { get; set; }

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

    }
}