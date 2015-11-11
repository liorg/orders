using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{

    public class NotifyMessage : IModifieder
    {
        public NotifyMessage()
        {

        }

        [Key]
        public Guid NotifyMessageId { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        public string ToUrl { get; set; }
        [Required]
        public Guid userId { get; set; }

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