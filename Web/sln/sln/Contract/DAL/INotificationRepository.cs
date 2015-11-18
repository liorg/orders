using Michal.Project.DataModel;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract.DAL
{
    public interface INotificationRepository
    {
        Task SendAsync(Guid? user, NotifyItem notifyItem);
       
    }
}
