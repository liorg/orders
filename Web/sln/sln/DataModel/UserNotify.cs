using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{

    public class UserNotify : IModifieder
    {
        public UserNotify()
        {

        }
         [Key]
        public Guid UserNotifyId { get; set; }
        [MaxLength]
       // [Key, Column(Order = 0)]
        public string DeviceId { get; set; }

     ///   [Key, Column(Order = 1)]
        public Guid UserId { get; set; }




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