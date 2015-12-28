using Michal.Project.Contract.DAL;
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

        public NotifyLogic(INotificationRepository notificationRepository)
        {
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
    }
}