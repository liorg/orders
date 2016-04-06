using Michal.Project.Contract.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class SyncObject : ISyncObject
    {
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
    }
}