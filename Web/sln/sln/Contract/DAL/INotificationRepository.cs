using Michal.Project.DataModel;
using Michal.Project.Models;
using Michal.Project.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract.DAL
{
    public interface INotificationRepository
    {
        Task<NotifiesView> GetNotifiesUserAsync(Guid userId, int? currentPage);
        Task Register(string userid, string deviceid);
        Task<NotifyItem> GetNotifyForCloudMessageAsync( string deviceid);
        Task Delete(Guid id);
    }
}
