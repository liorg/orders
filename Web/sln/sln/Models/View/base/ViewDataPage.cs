using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class ViewDataPageBase
    {
        public string Title { get; set; }
        public bool MoreRecord { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; }
        public Michal.Project.Helper.ClientViewType ClientViewType { get; set; }

    }

    public class ViewDataPage<T> : ViewDataPageBase
    {
        public T Items { get; set; }
    }
}