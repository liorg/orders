using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Helper
{
    public enum AlertStyle
    {
        Default = 0,
        Success = 4,
        Warning = 3,
        Info = 1,
        Error = 2,
        WaitingGet = 5,//timewait on get delievry
        WaitingSet = 6//timewait on set delievry
    }

}