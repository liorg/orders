using Michal.Project.Contract;
using Michal.Project.DataModel;
using Michal.Project.Helper;
using Michal.Project.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Security.Principal;
using Michal.Project.Contract.DAL;

namespace Michal.Project.Bll
{

    public class SyncLogic
    {

        readonly IShippingRepository _shippingRepository;
        readonly IUserRepository _userRepository;
        readonly ICommentRepository _commentRepostory;
        readonly ISyncRepository _syncRepository;

        public SyncLogic(IShippingRepository shippingRepository, 
            IUserRepository userRepository,
            ICommentRepository commentRepostory,
            ISyncRepository syncRepository
            )
        {
            _shippingRepository = shippingRepository;
            _userRepository = userRepository;
            _commentRepostory = commentRepostory;
            _syncRepository = syncRepository;

        }

        public async Task<IEnumerable<ItemSync<MobileShipVm>>> GetMyShipsAsync(Guid userid, string deviceid, string clientid)
        {
            var shipping = await _shippingRepository.GetShippingSyncByUserId(userid, deviceid, clientid);

            if (shipping == null) new List<ItemSync<MobileShipVm>>(); //throw new ArgumentNullException("shipping");
            return shipping;
        }

        public async Task<IEnumerable<ItemSync<MobileShipVm>>> GetMyDetail(Guid? userid)
        {
           var dataChanged=await _syncRepository.GetSyn(userid.Value, userid.Value, ObjectTableCode.USER);
           return null;
        }
    }
}