using Michal.Project.Contract;
using Michal.Project.Contract.DAL;
using Michal.Project.DataModel;
using Michal.Project.Helper;
using Michal.Project.Models;
using Michal.Project.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Bll
{
    public class NotifyLogic
    {
        readonly INotificationRepository _notificationRepository;
        readonly   IShippingRepository _shippingRepository;
       
        public NotifyLogic(INotificationRepository notificationRepository,IShippingRepository shippingRepository)
        {
            _shippingRepository=shippingRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task<NotifiesView> GetNotifiesUserAsync(UserContext user, int? currentPage)
        {
            return await _notificationRepository.GetNotifiesUserAsync(user.UserId, currentPage);
        }

        public async Task Register(string user, string deviceid)
        {
            await _notificationRepository.Register(user, deviceid);
        }

        public  async Task<NotifyItem> GetNotifyForCloudMessage( string deviceid)
        {
            return  await _notificationRepository.GetNotifyForCloudMessageAsync(deviceid);
        }

        public async Task Remove(Guid id)
        {
            await _notificationRepository.Delete(id);
        }

        public void NotifyShipUser(Shipping ship, IUserContext context)
        {
            if (ship.GrantRunner != null && ship.GrantRunner.Value == context.UserId)
            {
                ItemSync sync = new ItemSync();
             
                sync.LastUpdateRecord = DateTime.Now;
                sync.ObjectId = ship.ShippingId;
                sync.ObjectTableCode = ObjectTableCode.SHIP;
                sync.SyncStateRecord = SyncStateRecord.Change;
                sync.SyncStatus = SyncStatus.SyncFromServer;
                _shippingRepository.AddRecordTableAsync(context.UserId, sync);

            }

        }

        public void NotifyShipReplaceUser(Shipping ship, Guid oldUser, Guid newUser)
        {
        }
    }
}