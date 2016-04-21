using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{
    [Table("Member")]
    public class Member
    {
        public Member()
        {

        }

        [Key]
        public Guid MemberId { get; set; }


        [Index("IX_UserId", 1, IsUnique = true)]
        public Guid UserId1 { get; set; }
        [Index("IX_UserId", 2, IsUnique = true)]
        public Guid UserId2 { get; set; }


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