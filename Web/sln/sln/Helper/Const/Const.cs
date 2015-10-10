﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Helper
{
    public enum ClientViewType { Views = 1, Follows = 2, Users = 3, Search = 4, Report = 5 };
    
    public enum AlertStyle
    {
        Default=0,
        Success=4,
        Warning=3,
        Info=1,
        Error=2
    }
    public enum JobType { Admin = 1, Runner = 2, Client = 3 };

    public static class JobTitle
    {
        public const string Admin = "מנהל מערכת";
        public const string Client = "לקוח";
        public const string DeliveryBoy = "שליח";
    }

    public static class General
    {
        public const int MaxRecordsPerSearch = 100;
        public const int MaxRecordsPerPage = 10;
        public const string OrgWWW = "www";
        public const string OrgIDWWW = "00000000-0000-0000-0000-000000000002";
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
        public const string CancelByAdmin = "00000000-0000-0000-0000-000000000023";//5
        public const string AcceptByRunner = "00000000-0000-0000-0000-000000000019";//6
        public const string Arrived = "00000000-0000-0000-0000-000000000025";//7
        public const string AcceptByClient = "00000000-0000-0000-0000-000000000020";//8
        public const string NoAcceptByClient = "00000000-0000-0000-0000-000000000022";//9
        public const string Close = "00000000-0000-0000-0000-000000000021";//10
        public const string ArrivedSender = "00000000-0000-0000-0000-000000000025";//11
    }

    public class ProductType
    {
        public const string TimeWait = "00000000-0000-0000-0000-000000000001";
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
}