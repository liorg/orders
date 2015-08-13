
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Contract
{
    public interface IViewDays : IViewType
    {
        string FromDay { get; set; }
        string ToDay { get; set; }
        bool IsToday { get; set; }
    }
}