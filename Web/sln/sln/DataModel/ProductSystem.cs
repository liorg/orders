using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{

    public class ProductSystem : IModifieder
    {
        public ProductSystem()
        {

        }

        [Key]
        public Guid ProductSystemId { get; set; }

        public string Name { get; set; }

        [Required]
        public int ProductKey { get; set; }

        [Required]
        public int ProductTypeKey { get; set; } //1=SigBackType,2=Direction,3=TimeWait

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

        //  public bool IsCalculatingShippingInclusive { get; set; }


    }
}