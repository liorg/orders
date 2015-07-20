using sln.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace sln.DataModel
{

    public class PriceListForOrg : IModifieder
    {
        public PriceListForOrg()
        {

        }

        public Organization Organizations { get; set; }
        public Guid? Organizations_OrgId { get; set; }
        
        [Key]
        public Guid PriceListForOrgId { get; set; }

        public string Name { get; set; }

        public decimal MinTimeWait { get; set; }

        public decimal? Present { get; set; }

        public string Desc { get; set; }

        public DateTime BeginDate
        {
            get;
            set;
        }

        public DateTime? EndDate
        {
            get;
            set;
        }

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