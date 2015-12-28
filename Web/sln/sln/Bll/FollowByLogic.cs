using Michal.Project.Contract;
using Michal.Project.Contract.DAL;
using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Bll
{
    public class FollowByLogic
    {
        readonly IShippingRepository _shippingRepository;
        public FollowByLogic(IShippingRepository shippingRepository)
        {
            _shippingRepository = shippingRepository;
        }

        public async Task AddOwnerFollowBy(Shipping ship, Guid userid)
        {
            await _shippingRepository.AddOwnerFollowBy(ship, userid);
        }

        public async Task<Tuple<string>> GrantRunner(Guid shipId, Guid runner)
        {
            var ship = await _shippingRepository.GetShip(shipId);
           var  orderName = ship.Name;
            ship.GrantRunner = runner;
            await AddOwnerFollowBy(ship, runner);
            return new Tuple<string>(orderName);
        }

        public IEnumerable<Guid> GetUsersByShip(Shipping shipIncludeFollows)
        {
            HashSet<Guid> users = new HashSet<Guid>();

            foreach (var follow in shipIncludeFollows.FollowsBy)
            {
                users.Add(Guid.Parse(follow.Id));
            }
            return users;
        }
    }
}