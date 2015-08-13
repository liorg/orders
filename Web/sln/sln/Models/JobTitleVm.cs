using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
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