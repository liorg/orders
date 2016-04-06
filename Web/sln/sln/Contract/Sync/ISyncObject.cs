using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract.View
{
    public interface ISyncObject
    {
        int ObjectTableCode { get; set; }//ObjectTableCode
        Guid ObjectId { get; set; }
    }
}
