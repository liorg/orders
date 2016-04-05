using Michal.Project.Contract.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class RequestSync : ISync
    {

        public string DeviceId
        {
            get;
            set;
        }

        public string ClientId
        {
            get;
            set;
        }

        public Guid CurrentUserId
        {
            get;
            set;
        }
    }

    //public class RequestItemSync<T> : RequestSync, ISyncItem
    //{
    //    public T Model { get; set; }

    //    public int SyncStatus
    //    {
    //        get;
    //        set;
    //    }

    //    public int ObjectTableCode
    //    {
    //        get;
    //        set;
    //    }

    //    public int SyncStateRecord { get; set; }

    //    public Guid ObjectId
    //    {
    //        get;
    //        set;
    //    }

    //    public DateTime LastUpdateRecord { get; set; } 
    //}
}