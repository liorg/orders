using Michal.Project.Contract.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Michal.Project.Models
{
    public class Sync : ISync
    {
        public Guid UserId
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
    }
}