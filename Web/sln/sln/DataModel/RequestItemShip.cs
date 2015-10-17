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
      

        [ForeignKey("RequestShipping")]
        public Guid? RequestShipping_RequestShippingId { get; set; }
        public RequestShipping RequestShipping { get; set; }
        [Key]
        public Guid RequestItemShipId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }

        [Required]
        public int StatusCode { get; set; } // 1= request,2=requestByClient, ,3=OkResponse,4=CancelResponse

        public int ReqeustType { get; set; }//1= add (),2=discount

        public int PriceValueType { get; set; } //1 =fixed,2=%present
        public decimal? PriceValue { get; set; }

        public int PriceClientValueType { get; set; } //1 =fixed,2=%present
        public decimal? PriceClientValue { get; set; }

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