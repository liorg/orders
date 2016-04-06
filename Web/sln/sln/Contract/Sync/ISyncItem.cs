using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract.View
{
    public interface ISyncItem : ISync, ISyncObject
    {
        int SyncStatus { get; set; } // 0=No Sync,1=From Client ,2=From Server

        int SyncStateRecord { get; set; } //0=No,1=Add,2=Remove,3=Cancel
        DateTime LastUpdateRecord { get; set; }
    }
}
