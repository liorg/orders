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

        public async Task<NotifiesView> GetNotifiesUser(UserContext user, int? currentPage)
        {
            return await _notificationRepository.GetNotifiesUser(user.UserId, currentPage);


        }
    }
}