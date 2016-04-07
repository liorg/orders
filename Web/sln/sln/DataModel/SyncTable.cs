using Michal.Project.Contract;
using Michal.Project.Contract.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{
    [Table("SyncTable")]
    public class SyncTable : IModifieder, ISyncItem
    {
        public SyncTable()
        {


        }

        [Key]
        public Guid SyncTableId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public string DeviceId { get; set; }

        public string ClientId { get; set; }

        public DateTime LastUpdateRecord { get; set; } 

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


        [Required]
        public int SyncStatus
        {
            get;
            set;
        }
        [Required]
        public int ObjectTableCode
        {
            get;
            set;
        }
        [Required]
        public Guid ObjectId
        {
            get;
            set;
        }
        [Required]
        public int SyncStateRecord
        {
            get;
            set;
        }

        //public Guid CurrentUserId
        //{
        //    get;
        //    set;
        //}
    }
}