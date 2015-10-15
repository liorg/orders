using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class ItemPrice
    {
     
        [Display(Name = "שם")]
        public string NameProduct { get; set; }


        [Display(Name = "מחיר")]
        public double  Price { get; set; }

      
    }
}