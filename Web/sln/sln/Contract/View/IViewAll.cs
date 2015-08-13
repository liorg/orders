using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract
{
    public interface IViewAll : IViewType
    {
        string ShowAll { get; set; }
        bool BShowAll { get; set; }
    }
}
