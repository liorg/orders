using sln.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace sln.DataModel
{

    public class City : IModifieder
    {
        public City()
        {
            //ShippingsFrom = new HashSet<Shipping>();
            //ShippingsTo = new HashSet<Shipping>();

        }

        //public ICollection<Shipping> ShippingsFrom { get; set; }
        //public ICollection<Shipping> ShippingsTo { get; set; }

        [Key]
        public Guid CityId { get; set; }

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