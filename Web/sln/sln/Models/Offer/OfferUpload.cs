using Michal.Project.Contract.View;
using Michal.Project.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class OfferUpload : Offer, IShipView
    {
        public List<OfferItem> DataItems { get; set; }
    }

}