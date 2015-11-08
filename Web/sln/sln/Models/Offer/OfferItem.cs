using Michal.Project.Contract.View;
using Michal.Project.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class OfferItem : OfferClientItem
    {
        public Guid ObjectId { get; set; }
        public int ObjectIdType { get; set; }
    }

  
}