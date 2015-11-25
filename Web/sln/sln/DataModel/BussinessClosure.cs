using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{

    public class BussinessClosure : IModifieder, IBussinessClosure
    {
        public BussinessClosure()
        {
        
        }
      
        [Key]
        public Guid BussinessClosureId { get; set; }

        [Required]
        public Guid ShippingCompany { get; set; }

        public int DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        [Required]
        public bool IsDayOff { get; set; }

        public DateTime? SpecialDate { get; set; }

        [Required]
        public int Year { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

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