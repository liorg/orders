using sln.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln.Models
{
    public class JobTitleVm : IJob
    {

        public string JobType
        {
            get;
            set;
        }

        public string JobTitle
        {
            get;
            set;
        }
    }
}