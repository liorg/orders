using Michal.Project.Contract;
using Michal.Project.Contract.DAL;
using Michal.Project.DataModel;
using Michal.Project.Helper;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Bll
{
    public class FollowLogic
    {
        public async Task AddOwnerFollowBy(Shipping ship, IUserContext context, IDbSet<ApplicationUser> dbUser)//;//, ApplicationUser user)
        {
            var user = await dbUser.FirstOrDefaultAsync(u=>u.Id== context.UserId.ToString());
            ship.FollowsBy.Add(user);
        }
        public async Task AppendOwnerFollowBy(Shipping ship, IUserContext context, IDbSet<ApplicationUser> dbUser)
        {
            if (!ship.FollowsBy.Where(u => u.Id == context.UserId.ToString()).Any())
                await AddOwnerFollowBy(ship, context, dbUser);
        }

        public async Task AppendAdminFollowBy(Shipping ship, IEnumerable<IUserContext> admins, IDbSet<ApplicationUser> dbUser)
        {
            foreach (var admin in admins)
            {

                if (!ship.FollowsBy.Where(u => u.Id == admin.UserId.ToString()).Any())
                    await AddOwnerFollowBy(ship, admin, dbUser);
            }

        }

        
    }


    //public class NotifyLogic
    //{
    //    IShippingRepository _shippingRepository;

    //    public NotifyLogic(IShippingRepository shipRepo)
    //    {
    //        _shippingRepository = shipRepo;
    //    }

        
    //    {

    //    }
    //}
}