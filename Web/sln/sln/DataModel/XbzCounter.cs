using sln.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace sln.DataModel
{

    public class XbzCounter : IModifieder
    {
        public XbzCounter()
        {
         //   ShippingsFrom = new HashSet<Shipping>();
          //  ShippingsTo = new HashSet<Shipping>();

        }
        //[ForeignKey("CityFrom_CityId")]
        //public ICollection<Shipping> ShippingsFrom { get; set; }
        //[ForeignKey("CityTo_CityId")]
        //public ICollection<Shipping> ShippingsTo { get; set; }

        [Key]
        public Guid XbzCounterId { get; set; }

        public string Name { get; set; }

        public long LastNumber { get; set; }

        
    
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