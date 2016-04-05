using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Helper
{
    public static class SyncStatus
    {
        public const int NoSync = 0;
        public const int SyncFromClient = 1;
        public const int SyncFromServer = 2;
    }
}