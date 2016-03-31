using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Helper
{
    public enum ClientViewType { Views = 1, Follows = 2, Users = 3, Search = 4, Report = 5, General = 6 };

    public enum ObjectTypeCode
    {
        Product = 1, ProductSystem = 2, Distance = 3, ShipType = 4, Discount = 99, ExceptionPrice = 5
    }

    public enum AlertStyle
    {
        Default = 0,
        Success = 4,
        Warning = 3,
        Info = 1,
        Error = 2,
        WaitingGet = 5,//timewait on get delievry
        WaitingSet = 6//timewait on set delievry
    }

    public enum JobType { Admin = 1, Runner = 2, Client = 3 };

    public static class JobTitle
    {
        public const string Admin = "מנהל מערכת";
        public const string Client = "לקוח";
        public const string DeliveryBoy = "שליח";
    }

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

    public static class General
    {
        public const int MaxRecordsPerSearch = 100;
        public const int MaxRecordsPerPage = 10;
        public const string OrgWWW = "www";
        public const string OrgIDWWW = "00000000-0000-0000-0000-000000000002";

        public const string UnitMin = "דק'";
        public const string Unit = "יח'";

        public const string Shekel = "₪";

        public const string Empty = "---";

        public const double MAXMinutesExpiredApiToken = 30;
    }

    public static class CustomClaimTypes
    {
        public const string ShowAllView = "http://r.co.il/claims/ShowAllView";
        public const string DefaultView = "http://r.co.il/claims/DefaultView";
        public const string Tel = "http://r.co.il/claims/Tel";
        public const string JobTitle = "http://r.co.il/claims/JobTitle";
        public const string JobType = "http://r.co.il/claims/JobType";
        public const string City = "http://r.co.il/claims/City";
        public const string CityCode = "http://r.co.il/claims/CityCode";
        public const string Street = "http://r.co.il/claims/Street";
        public const string StreetCode = "http://r.co.il/claims/StreetCode";
        public const string Num = "http://r.co.il/claims/Num";
        public const string External = "http://r.co.il/claims/External";
        public const string UID = "http://r.co.il/claims/UID";
        public const string GrantUser = "http://r.co.il/claims/GrantUser";

        public const string Lat = "http://r.co.il/claims/Lat";
        public const string Lng = "http://r.co.il/claims/Lng";
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
        public const int ArrivedSender = 13;
    }

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

    public class ProductType
    {
        public const string TimeWait = "00000000-0000-0000-0000-000000000001";
        public const string ObjectIdExcpetionPrice = "10000000-0000-1111-1111-000000000001";
    }

    public class Notification
    {
        //public const int Default = 0;
        //public const int Error = 2;
        //public const int Warning = 1;
        //public const int Info = 3;
        //public const int Success = 4;
        public const string MessageConfirm = "יש לאשר את משלוח לפני קבלתו לחברת השליחים";
    }

    public class OfferVariables
    {
        public enum OfferStateCode { New = 1, Request = 2, End = 3, CancelOffer = 4, ConfirmException = 7,Close=10 }
    }

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

    public static class ObjectTableCode
    {
        public const int SHIP = 1;
        public const int COMMENT = 2;
    }
    public static class SyncStatus
    {
        public const int NoSync = 0;
        public const int SyncFromClient= 1;
        public const int SyncFromServer = 2;
    }
    public static class SyncStateRecord
    {
        public const int No = 0;
        public const int Add= 1;
        public const int Remove = 2;
        public const int Cancel = 3;
        public const int Change = 4;
    }
    
}