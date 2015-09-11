using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class AddressEditorViewModel
    {
        public int UId { get; set; }

        [Required]
        public string Street { get; set; }
        [Required]
        public string StreetCode { get; set; }

        [Required]
        public string City { get; set; }
        [Required]
        public string CityCode { get; set; }
    }
}