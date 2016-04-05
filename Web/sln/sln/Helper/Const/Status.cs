using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Helper
{
    public class Status
    {
        public const string Draft = "00000000-0000-0000-0000-000000000017";//1
        public const string ApporvallRequest = "00000000-0000-0000-0000-000000000016";//2
        public const string Cancel = "00000000-0000-0000-0000-000000000018";//3
        public const string Confirm = "00000000-0000-0000-0000-000000000024";//4
        public const string CancelByAdmin = "00000000-0000-0000-0000-000000000023";//4
        public const string AcceptByRunner = "00000000-0000-0000-0000-000000000019";//6
        public const string Arrived = "00000000-0000-0000-0000-000000000025";//7
        public const string AcceptByClient = "00000000-0000-0000-0000-000000000020";//8
        public const string NoAcceptByClient = "00000000-0000-0000-0000-000000000022";//8
        public const string Close = "00000000-0000-0000-0000-000000000021";//11
        public const string ArrivedSender = "00000000-0000-0000-0000-000000000026";//5

        public const int Max = 8;

    }
}