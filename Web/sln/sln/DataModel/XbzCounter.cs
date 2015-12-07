using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{

    public class XbzCounter : IModifieder
    {
        public XbzCounter()
        {
        

        }
      

        [Key]
        public Guid XbzCounterId { get; set; }

        public string Name { get; set; }

        public long LastNumber { get; set; }


        public virtual Organization Organizations { get; set; }
        [ForeignKey("Organizations")]
        public Guid? Organizations_OrgId { get; set; }

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