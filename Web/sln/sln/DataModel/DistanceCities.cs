using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Michal.Project.DataModel
{

    public class DistanceCities : IModifieder
    {
        public DistanceCities()
        {
       
        }



        [Key]
        [Column(Order = 1)]
        public string CityCode1 { get; set; }
        [Key]
        [Column(Order = 2)]
        public string CityCode2 { get; set; }

        public string Name { get; set; }
        public string Desc { get; set; }

        public string DestinationAddress { get; set; }
        public string OriginAddress { get; set; }

        public double DestinationLat { get; set; }
        public double DestinationLng { get; set; }

        public double OriginLat { get; set; }
        public double OriginLng { get; set; }

        public float DistanceValue { get; set; }
        public string DistanceText { get; set; }
  
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