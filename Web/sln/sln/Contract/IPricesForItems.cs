using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract
{
    public interface IPricesForItems
    {
        IEnumerable<PriceCalc> Get(Shipping ship);
    }
}
