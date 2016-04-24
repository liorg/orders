using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{
   

    [Table("MoreAddress")]
    public class MoreAddress
    {
        public MoreAddress()
        {

        }

        [Key]
        public Guid MoreAddressId { get; set; }

        [Required]
        public Guid ObjectId { get; set; }

        /*
           NONE = 0, SHIP = 1, COMMENT = 2, USER = 3,COMPANY = 4,ORG = 5;
         */
        [Required]
        public int ObjectTableCode
        {        
            get;     
            set;
        } 
        public Address Address { get; set; }

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
        public string Tel1 { get; set; }
      //  public string Tel2 { get; set; }

        public string Name1 { get; set; }

        public string Name2 { get; set; }

    }
}