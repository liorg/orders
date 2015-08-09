﻿using sln.Contract;
using sln.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace sln.Bll
{
    public class FollowLogic
    {
        public async Task AddOwnerFollowBy(Shipping ship, IUserContext context, IDbSet<ApplicationUser> dbUser)//;//, ApplicationUser user)
        {
            var user = await dbUser.FirstOrDefaultAsync(u=>u.Id== context.UserId.ToString());
            ship.FollowsBy.Add(user);
        }
    }
}