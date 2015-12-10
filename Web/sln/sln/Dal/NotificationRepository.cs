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

        public async Task<NotifiesView> GetNotifiesUserAsync(Guid userId, int? currentPage)
        {
            NotifiesView notifiesView = new NotifiesView();
            int page = currentPage.HasValue ? currentPage.Value : 1;
            var query = _context.NotifyMessage.Where(u => u.UserId == userId && u.IsActive == true).AsQueryable();
            List<NotifyItem> items = await query.OrderByDescending(crt => crt.CreatedOn).Skip((page - 1) * Helper.General.MaxRecordsPerPage).Take(General.MaxRecordsPerPage).Select(
                 m => new NotifyItem
                 {
                     Id = m.NotifyMessageId,
                     Title = m.Title,
                     IsRead = m.IsRead,
                     Body = m.Body,
                     Url = m.ToUrl,
                     CreatedOn = m.CreatedOn.HasValue ? m.CreatedOn.Value : DateTime.MinValue

                 }).ToListAsync();

            var total = await query.CountAsync();

            var hasMoreRecord = total > (page * Helper.General.MaxRecordsPerPage);
            notifiesView.CurrentPage = page;
            notifiesView.ClientViewType = ClientViewType.General;
            notifiesView.MoreRecord = hasMoreRecord;
            notifiesView.Items = items;
            notifiesView.Total = total;
            return notifiesView;
        }

        public async Task Register(string userid, string deviceid)
        {
            var dt = DateTime.Now;
            var registersAlreadyDevices = await _context.UserNotify.Where(u => u.DeviceId == deviceid && u.IsActive == true).ToListAsync();

            foreach (var removeRegisterDevice in registersAlreadyDevices)
            {
                _context.UserNotify.Remove(removeRegisterDevice);
            }

            var userId = Guid.Parse(userid);
            var removes = await _context.UserNotify.Where(dd => dd.UserId == userId).ToListAsync();
            foreach (var remove in removes)
            {
                _context.UserNotify.Remove(remove);
            }

            _context.UserNotify.Add(new DataModel.UserNotify
            {
                UserNotifyId = Guid.NewGuid(),
                CreatedOn = dt,
                DeviceId = deviceid,
                IsActive = true,
                ModifiedOn = dt,
                UserId = Guid.Parse(userid)
            });
        }

        public async Task<NotifyItem> GetNotifyForCloudMessageAsync(string deviceid)
        {
            var dt = DateTime.Now;
            var url = System.Configuration.ConfigurationManager.AppSettings["server"].ToString();
            var path = "/Notification/";
            var model = new NotifyItem();
            model.Url = url + path;
            model.Title = "מערכת הודעות זמן אמת";
            var user = await (from un in _context.UserNotify
                              join u in _context.Users
                              on un.UserId.ToString() equals u.Id
                              where un.DeviceId == deviceid
                              select u).FirstOrDefaultAsync();

            if (user != null)
            {
                var userId = Guid.Parse(user.Id);
                var fullname = user.FirstName + " " + user.LastName;
                model.Body = "שים לב יש לך הודעות חדשות";
                var notifyMessages = await _context.NotifyMessage.Where(u => u.UserId == userId && u.IsActive == true && u.IsRead == false).ToListAsync();
                if (notifyMessages.Count() == 0)
                {
                    model.Body = fullname + "," + "אן הודעות מהמערכת !";
                    return model;
                }

                if (notifyMessages.Count() > 1)
                    model.Body = fullname + "," + "יש לך מספר הודעות " + notifyMessages.Count().ToString() + " חדשות ";
                else
                {
                    var notifyMessageFirst = notifyMessages.First();
                    model.Body = fullname + "," + notifyMessageFirst.Body;
                    model.Url = notifyMessageFirst.ToUrl;
                    model.Id = notifyMessageFirst.NotifyMessageId;
                }

                foreach (var notifyMessage in notifyMessages)
                {
                    notifyMessage.IsRead = true;
                    _context.Entry<NotifyMessage>(notifyMessage).State = EntityState.Modified;
                }
            }
            else
                model.Body = "..אן לך הודעות חדשות";
            return model;
        }
    }
}

