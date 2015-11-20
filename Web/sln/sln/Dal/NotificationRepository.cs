using Michal.Project.Contract.DAL;
using Michal.Project.Dal;
using Michal.Project.DataModel;
using Michal.Project.Models;
using Michal.Project.Models.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using Michal.Project.Helper;
namespace Michal.Project.Dal
{
    public class NotificationRepository : INotificationRepository
    {
        ApplicationDbContext _context;
        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotifiesView> GetNotifiesUser(Guid userId, int? currentPage)
        {
            NotifiesView notifiesView = new NotifiesView();
            int page = currentPage.HasValue ? currentPage.Value : 1;
            var query = _context.NotifyMessage.Where(u => u.UserId == userId && u.IsActive == true).AsQueryable();
            List<NotifyItem> items = await query.OrderByDescending(crt=>crt.CreatedOn).OrderByDescending(ord => ord.IsRead).Skip((page - 1) * Helper.General.MaxRecordsPerPage).Take(General.MaxRecordsPerPage).Select(
                 m => new NotifyItem
                 {
                     Id = m.NotifyMessageId,
                     Title = m.Title,
                     IsRead = m.IsRead,
                     Body = m.Body,
                     Url = m.ToUrl,
                     CreatedOn= m.CreatedOn.HasValue? m.CreatedOn.Value:DateTime.MinValue

                 }).ToListAsync();

            var total = await query.CountAsync();
            
            var hasMoreRecord = total > (page * Helper.General.MaxRecordsPerPage);
            notifiesView.CurrentPage = page;
            notifiesView.ClientViewType = ClientViewType.Views;
            notifiesView.MoreRecord = hasMoreRecord;
            notifiesView.Items = items;
            notifiesView.Total = total;
            return notifiesView;
        }
    }
}
