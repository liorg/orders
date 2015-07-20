using sln.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace sln.DataModel
{

    public class Distance : IModifieder
    {
        public Distance()
        {
           Organizations = new HashSet<Organization>();
           Shippings = new HashSet<Shipping>();
           PriceLists = new HashSet<PriceList>();
           Discounts = new HashSet<Discount>();
        }
        
        public ICollection<Organization> Organizations { get; set; }

        [ForeignKey("Distance_DistanceId")]
        public ICollection<PriceList> PriceLists { get; set; }

        [ForeignKey("Distance_DistanceId")]
        public ICollection<Shipping> Shippings { get; set; }

        // many to many
        public ICollection<Discount> Discounts { get; set; }

        [Key]
        public Guid DistanceId { get; set; }

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