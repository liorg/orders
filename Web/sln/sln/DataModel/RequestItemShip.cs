using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{
    public class RequestItemShip : IModifieder  
    {
        public RequestItemShip()
        {

        }

        public Guid? ObjectTypeId { get; set; }
        public int? ObjectTypeIdCode { get; set; }  //discount=1, addprise=2

        [ForeignKey("RequestShipping")]
        public Guid? RequestShipping_RequestShippingId { get; set; }
        public RequestShipping RequestShipping { get; set; }
        [Key]
        public Guid RequestItemShipId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }

        public int PriceValueType { get; set; } //1 =fixed,2=%present
        public decimal? PriceValue { get; set; }
        public decimal? ProductValue { get; set; }

        public int Amount { get; set; }
        public bool IsDiscount { get; set; }
        public int QuntityType { get; set; } //0 =unit,1=min

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