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
using Michal.Project.Contract.View;

namespace Michal.Project.Bll
{

    public class SyncLogic
    {

        readonly IShippingRepository _shippingRepository;
        readonly IUserRepository _userRepository;
        readonly ICommentRepository _commentRepostory;
        readonly ISyncRepository _syncRepository;
        readonly INotificationRepository _notificationRepository;
        readonly ISupplierRepostory _supplierRepostory;

        public SyncLogic(IShippingRepository shippingRepository, 
            IUserRepository userRepository,
            ICommentRepository commentRepostory,
            ISyncRepository syncRepository,
            INotificationRepository notificationRepository, ISupplierRepostory supplierRepostory
            )
        {
            _shippingRepository = shippingRepository;
            _userRepository = userRepository;
            _commentRepostory = commentRepostory;
            _syncRepository = syncRepository;
            _notificationRepository = notificationRepository;
            _supplierRepostory = supplierRepostory;
        }

        public async Task<IEnumerable<ItemSync<MobileShipVm>>> GetMyShipsAsync(Guid userid, string deviceid, string clientid)
        {
            var shipping = await _shippingRepository.GetShippingSyncByUserId(userid, deviceid, clientid);

            if (shipping == null) new List<ItemSync<MobileShipVm>>(); //throw new ArgumentNullException("shipping");
            return shipping;
        }

        public async Task<WhoAmI> GetWhoAmI(Guid userid)
        {
            return await _userRepository.GetMyDetail(userid);
        }

        public async Task<ItemSync<WhoAmI>> GetMyDetail(ISyncItem request)
        {
            var itemSync = new ItemSync<WhoAmI>();

            var dataChanged = await _syncRepository.GetSyn(request.UserId, request.ObjectId, ObjectTableCode.USER);
            if (dataChanged.Any())
            {
                itemSync.SyncObject = await _userRepository.GetMyDetail(request.UserId);
                itemSync.ClientId = request.ClientId;
                itemSync.DeviceId = request.DeviceId;
                itemSync.LastUpdateRecord = DateTime.Now;
                itemSync.ObjectId = request.ObjectId;
                itemSync.ObjectTableCode = request.ObjectTableCode;
                itemSync.SyncStateRecord = request.SyncStateRecord;
            }
            else
            {
                itemSync.SyncStatus = SyncStatus.NoSync;
            }
            return itemSync;
        }

        public async Task DeleteSyncFlags(ISyncItem request)
        {
            await _syncRepository.DeleteUnused(request);
        }

        public async Task<IEnumerable<ISync>> GetDevicesByUserId(Guid userid)
        {
            return await _notificationRepository.GetDevicesByUserId(userid);
        }

        public async Task SyncFlagOn(ISyncItem request)
        {
           await _syncRepository.SyncOn(request);
        }

        public List<Runner> GetRunners()
        {
            return _supplierRepostory.GetRunners();
        }
    }
}