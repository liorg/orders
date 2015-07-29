using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln.Models
{
    public class UserProfiler
    {
        public bool AllowConfirm { get; set; }  // allow confirm account manager
        public bool AllowAccept { get; set; }// accept and send to delivery
        public bool AllowSender { get; set; } // running 
        public bool IsAdmin { get; set; }
    }
}