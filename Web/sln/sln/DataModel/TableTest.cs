using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{
    public class TableTest : IModifieder
    {
        [Key]
        public Guid TableTestId { get; set; }

        public Address ShippingAddress { get; set; }
        public Address BillingAddress { get; set; }

        public TableTest()
        {


            this.BillingAddress = new Address();
            this.ShippingAddress = new Address();

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