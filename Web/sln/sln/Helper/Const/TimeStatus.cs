using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Helper
{
    public class TimeStatus
    {
        public const int New = 1;
        public const int ApporvallRequest = 2;
        public const int Cancel = 3;
        public const int Confirm = 4;
        public const int CancelByAdmin = 5;
        public const int AcceptByRunner = 6;
        public const int Arrived = 7;
        public const int AcceptByClient = 8;
        public const int NoAcceptByClient = 9;
        public const int Close = 10;
        public const int PrevStep = 11;
        public const int ChangePrice = 12;
        public const int ArrivedSender = 13;
    }
}