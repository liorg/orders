using Michal.Project.Contract;
using Michal.Project.Contract.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models.View
{
    public class UsersView : ViewDataPage<IEnumerable<EditUserViewModel>>, IView
    {

        public Guid Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}