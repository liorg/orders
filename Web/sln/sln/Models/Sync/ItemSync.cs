using Michal.Project.Contract.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class ItemSync : ISyncItem
    {
        public int SyncStatus
        {
            get;
            set;
        }

        public int ObjectTableCode
        {
            get;
            set;
        }

        public Guid ObjectId
        {
            get;
            set;
        }

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

        public int SyncStateRecord { get; set; }

        public DateTime LastUpdateRecord { get; set; }

        public Guid CurrentUserId
        {
            get;
            set;
        }
    }
   
    public class ItemSync<T> : ItemSync
    {
        public T Model { get; set; }

      
    }
}