using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{
    [Table("Friend")]
    public class Friend : IModifieder
    {
        [Key]
        [Column(Order = 1)]
        public Guid UserId1 { get; set; }
        [Key]
        [Column(Order = 2)]
        public Guid UserId2 { get; set; }


        public string Name { get; set; }

    //    public string Desc { get; set; }

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