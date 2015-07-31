using sln.Helper;
using sln.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln.Bll
{
    public class ViewLogic
    {
        public Tuple<int, bool> GetViewPropByRole(IRole roles)
        {
            bool showAll = true;
            int defaultView = TimeStatus.New;
            if (roles.IsCreateOrder)
                showAll = false;
            if (roles.IsAcceptOrder){
                showAll = true;
                defaultView = TimeStatus.ApporvallRequest;
            }
            if (roles.IsOrgMangager)
            {
                showAll = true;
                defaultView = TimeStatus.AcceptByRunner;
            }
            if (roles.IsRunner)
            {
                showAll = false;
                defaultView = TimeStatus.AcceptByRunner;
            }
            if (roles.IsAdmin)
            {
                showAll = true;
                defaultView = TimeStatus.AcceptByRunner;
            }

            return new Tuple<int, bool>(defaultView, showAll); ;

        }
    }
}