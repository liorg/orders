using sln.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace sln.DataModel
{

    public class Comment : IModifieder
    {
        public Comment()
        {
         //   ShippingsFrom = new HashSet<Shipping>();
          //  ShippingsTo = new HashSet<Shipping>();

        }
        //[ForeignKey("CityFrom_CityId")]
        //public ICollection<Shipping> ShippingsFrom { get; set; }
        //[ForeignKey("CityTo_CityId")]
        //public ICollection<Shipping> ShippingsTo { get; set; }

        public virtual Shipping Shipping { get; set; }
        [ForeignKey("Shipping")]
        public Guid? Shipping_ShippingId { get; set; }


        [Key]
        public Guid CommentId { get; set; }

        public string JobType { get; set; }

        public string JobTitle { get; set; }

        public string Name { get; set; }

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