using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract
{
    public interface IPriceCalc
    {
        /// <summary>
        /// update calc on ship
        /// </summary>
        /// <param name="priceLists"></param>
        /// <param name="discounts"></param>
        /// <param name="ship"></param>
     //   void ReCalc(IEnumerable<PriceList> priceLists, IEnumerable<Discount> discounts, Shipping ship);
    }
}
