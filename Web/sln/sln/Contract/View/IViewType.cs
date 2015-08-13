using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract
{
    public interface IViewType
    {
         Michal.Project.Helper.ClientViewType ClientViewType { get; set; }
    }
}
