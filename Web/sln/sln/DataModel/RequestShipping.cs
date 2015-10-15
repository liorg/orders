using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{
    public class RequestShipping : IModifieder
    {
        public RequestShipping()
        {
            Discount = new HashSet<Discount>();
        }
        public string Name { get; set; }
        public string Desc { get; set; }
        // many to many
        public ICollection<Discount> Discount { get; set; }
        [Key]
        public Guid RequestShippingId { get; set; }

        [ForeignKey("ShippingCompany")]
        public Guid? ShippingCompany_ShippingCompanyId { get; set; }
        public ShippingCompany ShippingCompany { get; set; }

        public virtual Organization Organizations { get; set; }
        [ForeignKey("Organizations")]
        public Guid? Organizations_OrgId { get; set; }

        [Required]
        public int StatusCode { get; set; } //1 =OrderRequest,2=OrderResponse,3=OfferRequest,4=OfferResponse,5=Commit

       
        //1 = OrderReqest,1=Order
        //2 = OrderResponse => 2=Ok,3=Cancel
        //3 = OfferRequest=>4=>Offer
        //4 = OfferResponse=>5=Ok,6=Cancel
        //5=Commit=>6=Ok,7=Cancel (Commit By Client done)
        [Required]
        public int StatusReasonCode { get; set; }

        [Column(TypeName = "Money")]
        public decimal Price { get; set; }

        [Column(TypeName = "Money")]
        public decimal DiscountPrice { get; set; }


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

        public Guid? OwnerId { get; set; }

        public bool IsActive
        {
            get;
            set;
        }

    }
}