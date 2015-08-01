using sln.Contract;
using sln.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln.Bll
{
    public class ViewLogic
    {
        public void SetViewerUserByRole(IRole source,IViewerUser target)
        {
            bool showAll = true;
            int defaultView = TimeStatus.New;
            if (source.IsCreateOrder)
                showAll = false;
            if (source.IsAcceptOrder)
            {
                showAll = true;
                defaultView = TimeStatus.ApporvallRequest;
            }
            if (source.IsOrgMangager)
            {
                showAll = true;
                defaultView = TimeStatus.AcceptByRunner;
            }
            if (source.IsRunner)
            {
                showAll = false;
                defaultView = TimeStatus.AcceptByRunner;
            }
            if (source.IsAdmin)
            {
                showAll = true;
                defaultView = TimeStatus.AcceptByRunner;
            }
            target.DefaultView = defaultView;
            target.ViewAll = showAll;


        }
    }
}