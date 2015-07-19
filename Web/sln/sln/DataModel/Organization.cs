using sln.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sln.DataModel
{
    public class Organization : IModifieder
    {
        public Organization()
        {
            PriceListsForOrg = new HashSet<PriceListForOrg>();
            Users = new HashSet<ApplicationUser>();
            Distances = new HashSet<Distance>();
            PriceLists = new HashSet<PriceList>();

        }
        public ICollection<PriceListForOrg> PriceListsForOrg { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<PriceList> PriceLists { get; set; }
        public ICollection<Distance> Distances { get; set; }

        [Key]
        public Guid OrgId { get; set; }

        public string Name { get; set; }

        public string Domain { get; set; }

        public DateTime? CreatedOn
        {
            get; set;
        }

        public DateTime? ModifiedOn
        {
            get; set;
        }

        public Guid? CreatedBy
        {
            get;  set;
        }

        public Guid? ModifiedBy
        {
            get; set;
        }

        public bool IsActive
        {
            get;  set;
        }
    }
}