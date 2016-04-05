using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{

    public class Comment : IModifieder, IJob
    {
        public Comment()
        {
        
        }
      

       
        //http://www.entityframeworktutorial.net/code-first/foreignkey-dataannotations-attribute-in-code-first.aspx
        [ForeignKey("Shipping")]
        public Guid? Shipping_ShippingId { get; set; }
        public virtual Shipping Shipping { get; set; }


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