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
          //  Organizations = new HashSet<Organization>();
           // Distances = new HashSet<Distance>();
        }

        PriceList PriceList { get; set; }

        [Key]
        public Guid DiscountId { get; set; }

        public decimal Present { get; set; }

        public int? MinQuantity { get; set; }

        public int? MaxQuantity { get; set; }

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