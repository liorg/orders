using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Helper
{
    public static class SyncStateRecord
    {
        public const int No = 0;
        public const int Add = 1;
        public const int Remove = 2;
        public const int Cancel = 3;
        public const int Change = 4;
    }
}