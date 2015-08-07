using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln.Helper
{

   // public enum JobType {Admin=1,}
    public static class General
    {

        public const int MaxRecordsPerPage = 3;
        public const string OrgWWW = "www";
    }
    public static class CustomClaimTypes
    {
        public const string ShowAllView = "http://r.co.il/claims/ShowAllView";
        public const string DefaultView = "http://r.co.il/claims/DefaultView";
        public const string Tel = "http://r.co.il/claims/Tel";
    }
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
    }
    public class Status
    {
        public const string Draft = "00000000-0000-0000-0000-000000000017";//1
        public const string ApporvallRequest = "00000000-0000-0000-0000-000000000016";//2
        public const string Cancel = "00000000-0000-0000-0000-000000000018";//3
        public const string Confirm = "00000000-0000-0000-0000-000000000024";//4
        public const string CancelByAdmin = "00000000-0000-0000-0000-000000000023";//5
        public const string AcceptByRunner = "00000000-0000-0000-0000-000000000019";//6
        public const string Arrived = "00000000-0000-0000-0000-000000000025";//7
        public const string AcceptByClient = "00000000-0000-0000-0000-000000000020";//8
        public const string NoAcceptByClient = "00000000-0000-0000-0000-000000000022";//9
        public const string Close = "00000000-0000-0000-0000-000000000021";//10

    }

    public class ProductType
    {
        public const string TimeWait = "00000000-0000-0000-0000-000000000001";

    }
    public class Notification
    {
        public const int Default = 0;
        public const int Error = 2;
        public const int Warning = 1;
        public const int Info = 3;
        public const int Success = 4;
        public const string MessageConfirm = "יש לאשר את משלוח לפני קבלתו לחברת השליחים";
    }
}