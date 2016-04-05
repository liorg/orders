using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Helper
{
    public class DefaultShip
    {
        public enum DType { Distance, ShipType, DefaultCompany }
        public List<Tuple<DType, string, Guid>> items;// = new List<Tuple<string, string, Guid>>();
        // Dictionary<Guid, string> _values;
        public DefaultShip()
        {
            items = new List<Tuple<DType, string, Guid>>();
            items.Add(new Tuple<DType, string, Guid>(DType.Distance, "מרחב דן", Guid.Parse("00000000-0000-0000-0000-000000000004")));
            items.Add(new Tuple<DType, string, Guid>(DType.ShipType, "שליחות רגילה", Guid.Parse("00000000-0000-0000-0000-000000000001")));
            items.Add(new Tuple<DType, string, Guid>(DType.DefaultCompany, "רן שליחויות", Guid.Parse("00000000-0000-0000-0000-000000000001")));
        }

    }

}