using sln.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sln.DataModel
{

    public class Distance : IModifieder
    {
        public Distance()
        {
            Organizations = new HashSet<Organization>();
        }
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

        public ICollection<Organization> Organizations { get; set; }

    }
}