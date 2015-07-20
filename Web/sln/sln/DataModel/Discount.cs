using sln.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace sln.DataModel
{

    public class Discount : IModifieder
    {
        public Discount()
        {
       
        }

        PriceList PriceList { get; set; }
        public Guid? PriceList_PriceListId  { get; set; }

        [Key]
        public Guid DiscountId { get; set; }

        public decimal Present { get; set; }

        public Distance Distance { get; set; }
        public Guid? Distance_DistanceId { get; set; }

        public int? MinQuantityOneWay { get; set; }

        public string Name { get; set; }

        public decimal Precent { get; set; }

        public string Desc { get; set; }

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