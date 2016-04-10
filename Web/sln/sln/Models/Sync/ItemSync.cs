using Michal.Project.Contract.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{


    public class ItemSync : Sync,ISyncItem
    {
        public ItemSync()
        {

        }
        public ItemSync(ISyncItem copy)
        {
            this.ClientId = copy.ClientId;
            this.DeviceId = copy.DeviceId;
            this.UserId = copy.UserId;
            this.LastUpdateRecord = copy.LastUpdateRecord;
            this.ObjectId = copy.ObjectId;
            this.ObjectTableCode = copy.ObjectTableCode;
            this.SyncStateRecord = copy.SyncStateRecord;
            this.SyncStatus = copy.SyncStatus;

        }
        public ItemSync(ISyncObject copy)
        {
          
            this.ObjectId = copy.ObjectId;
            this.ObjectTableCode = copy.ObjectTableCode;
        
           
        }

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

        //public string DeviceId
        //{
        //    get;
        //    set;
        //}

        //public string ClientId
        //{
        //    get;
        //    set;
        //}

        public int SyncStateRecord { get; set; }

        public DateTime LastUpdateRecord { get; set; }

        //public Guid UserId
        //{
        //    get;
        //    set;
        //}
    }
   
    public class ItemSync<T> : ItemSync
    {
        public T SyncObject { get; set; }

      
    }
    

}