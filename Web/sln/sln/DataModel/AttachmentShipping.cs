﻿using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{

    public class AttachmentShipping : IModifieder
    {
        public AttachmentShipping()
        {
        }
 
        public virtual Shipping Shipping { get; set; }
        [ForeignKey("Shipping")]
        public Guid? Shipping_ShippingId { get; set; }


        [Key]
        public Guid CommentId { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        [Required]
        public bool IsSign { get; set; }
        
        public string TypeMime { get; set; }

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