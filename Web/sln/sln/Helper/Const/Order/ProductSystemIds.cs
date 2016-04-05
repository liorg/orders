using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Helper
{
    public static class ProductSystemIds
    {
        public const int MinAmountTimeWaitInMIn = 15;
        public enum ProductSystemType { TimeWait = 3, Back = 1, Direction = 2 };
        public const string TimeWaitSet = "00000000-0000-0000-0000-000000000001";
        public const string TimeWaitGet = "00000000-0000-0000-0000-000000000002";

        public const string NoBack = "00000000-0000-0000-0000-000000000005";
        public const string NoBackSigAuto = "00000000-0000-0000-0000-000000000003";
        public const string Back = "00000000-0000-0000-0000-000000000004";

    }

}