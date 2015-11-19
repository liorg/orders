using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class MessageForUsers
    {
        public NotifyItem NotifyItem { get; set; }
        public List<Guid> Users { get; set; }
    }
}