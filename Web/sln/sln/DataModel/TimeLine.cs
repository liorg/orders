using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{

    public class TimeLine : IModifieder
    {
        public TimeLine()
        {

        }

        public virtual Shipping Shipping { get; set; }
        [ForeignKey("Shipping")]
        public Guid? Shipping_ShippingId { get; set; }

        [Key]
        public Guid TimeLineId { get; set; }

        public int Status { get; set; }

        public virtual StatusShipping StatusShipping { get; set; }
        [ForeignKey("StatusShipping")]
        public Guid? StatusShipping_StatusShippingId { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public string DescHtml { get; set; }

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

        public Guid? ApprovalRequest { get; set; }

        public Guid? ApprovalShip { get; set; }

        public Guid? OwnerShip { get; set; }
    }
}