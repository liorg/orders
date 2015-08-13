using Michal.Project.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models.View
{
    public class UsersView : ViewDataPage<IEnumerable<EditUserViewModel>>
    {

        public string FromDay
        {
            get;
            set;
        }

        public string ToDay
        {
            get;
            set;
        }

        public bool IsToday
        {
            get;
            set;
        }

        public string ShowAll
        {
            get;
            set;
        }

        public bool BShowAll
        {
            get;
            set;
        }
    }
}