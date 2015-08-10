using sln.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln.Models
{
    public class CommentVm : IJob
    {
        public DateTime CreatedOn { get; set; }
        public string JobTitle { get; set; }
        public string Desc { get; set; }
        public string JobType { get; set; }

        public string DateComment
        {
            get
            {
                if (CreatedOn.Date == DateTime.Now.Date)
                   return CreatedOn.Date.ToString("hh:mm");

                return CreatedOn.Date.ToString("dd-MM-yy");
            }
        }
    }
}