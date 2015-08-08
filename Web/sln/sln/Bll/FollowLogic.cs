using sln.Contract;
using sln.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace sln.Bll
{
    public class FollowLogic
    {
        public void AddOwnerFollowBy(Shipping ship,IUserContext context, IDbSet<ApplicationUser> dbset)//;//, ApplicationUser user)
        {
            var user = dbset.Find(context.UserId.ToString());
            ship.FollowsBy.Add(user);
        }
    }
}