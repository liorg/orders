using sln.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace sln.DataModel
{

    public class TimeLine : IModifieder
    {
        public TimeLine()
        {
            Shippings= new HashSet<Shipping>();
   

        }

        public ICollection<Shipping> Shippings { get; set; }

        [Key]
        public Guid TimeLineId { get; set; }

        public StatusTimeLine StatusTimeLine { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public string DescHtml { get; set; }

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