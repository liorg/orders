using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class Notify
    {
        public List<NotifyItem> Items { get; set; }
        public string Url { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public bool MoreThenOne
        {
            get
            {
                if (Items != null && Items.Any() && Items.Count > 1)
                {
                    return true;
                }
                return false;
            }
        }
    }

    public class NotifyItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Url { get; set; }
    }
}