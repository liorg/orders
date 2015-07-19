﻿using sln.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace sln.DataModel
{

    public class PriceList : IModifieder
    {
        public PriceList()
        {
          //  Organizations = new HashSet<Organization>();
           // Distances = new HashSet<Distance>();
        }

        Product Product { get; set; }
        public Organization Organizations { get; set; }

        [Key]
        public Guid PriceListId { get; set; }

        [Column(TypeName = "Money")]
        public decimal Price { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public DateTime BeginDate
        {
            get;
            set;
        }

        public DateTime? EndDate
        {
            get;
            set;
        }

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