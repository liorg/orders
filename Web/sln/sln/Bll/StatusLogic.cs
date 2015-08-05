﻿using sln.DataModel;
using sln.Helper;
using sln.Models;
using sln.Models.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sln.Bll
{
    public class StatusLogic
    {
        public void RemoveOrder(StatusRequestBase requestBase)
        {
            StatusRequest request = new StatusRequest(requestBase);
            var text = "הזמנה בוטלה ע''י" + " " + request.UserContext.FullName;
            request.Title = text;
            request.Desc = text;
            request.Status = TimeStatus.Cancel;
            request.NotifyType = Notification.Error;
            request.StatusShipping = Guid.Parse(Helper.Status.Cancel);

            request.Ship.CancelByUser = request.UserContext.UserId;
            ChangeStatus(request);
        }

        public void ApprovalRequest(StatusRequestBase requestBase)
        {
            StatusRequest request = new StatusRequest(requestBase);
            var text = "הזמנה אושרה " + " " + request.Ship.Name + " " + "בתאריך " + request.CurrentDate.ToString("dd/MM/yyyy hh:mm");
            request.Title = text;
            request.Desc = text;
            
            request.NotifyType = Notification.Info;
            request.Status = TimeStatus.ApporvallRequest;
            request.StatusShipping = Guid.Parse(Helper.Status.ApporvallRequest);

            request.Ship.ApprovalRequest = request.UserContext.UserId;
            ChangeStatus(request);
        }

        public void ConfirmRequest(StatusRequestBase requestBase, Func<string,string> GetRunner)
        {
            StatusRequest request = new StatusRequest(requestBase);

            var grantToText = "";
            if (!String.IsNullOrEmpty(request.AssignTo))
            {
               request.Ship.GrantRunner = Guid.Parse(request.AssignTo);
               grantToText = GetRunner(request.AssignTo);  //cache.GetRunners(context).Where(run => run.Id == assignTo).Select(run2 => run2.FullName).FirstOrDefault();
            }
            else
            {
                request.Ship.GrantRunner = request.UserContext.UserId;
                grantToText = request.UserContext.FullName;
            }

            var title = "הזמנה אושרה ע'' חברת השליחות" + " ע''י " + request.UserContext.FullName + " (" + request.UserContext.EmpId + ")";
            var text = title + System.Environment.NewLine + " " + "הזמנה אושרה " + " " + request.Ship.Name + " " + "בתאריך " + request.CurrentDate.ToString("dd/MM/yyyy hh:mm") + " והועברה לשליח" + " " + grantToText;

            request.Title = title;
            request.Desc = text;
            request.NotifyType = Notification.Info;
            request.Status = TimeStatus.Confirm;
            request.StatusShipping = Guid.Parse(Helper.Status.Confirm);

            request.Ship.ApprovalShip = request.UserContext.UserId;
            ChangeStatus(request);
        }

        public void Accept(StatusRequestBase requestBase)
        {

            StatusRequest request = new StatusRequest(requestBase);
            var user=request.UserContext;
            var ship = request.Ship;
            var title = "הזמנה  התקבלה " + " ע''י השליח " + user.FullName + " (" + user.EmpId + ")";
            var text = title + System.Environment.NewLine + " " + " מספר הזמנה " + " " + ship.Name + " " + "בתאריך " + request.CurrentDate.ToString("dd/MM/yyyy hh:mm");
            
            request.Title = title;
            request.Desc = text;
            request.NotifyType = Notification.Info;
            request.Status = TimeStatus.AcceptByRunner;
            request.StatusShipping = Guid.Parse(Helper.Status.AcceptByRunner);

            request.Ship.BroughtShipmentCustomer = request.UserContext.UserId;
            ChangeStatus(request);
        }

        public void CancelRequest(StatusRequestBase requestBase)
        {
            StatusRequest request = new StatusRequest(requestBase);
            var user = request.UserContext;
            var ship = request.Ship;
            var currentDate = request.CurrentDate;
            var text = "הזמנה לא מאושרת" + " " + ship.Name + " " + "בתאריך " + currentDate.ToString("dd/MM/yyyy hh:mm");
            request.Title = text;
            request.Desc = text;
            request.Status = TimeStatus.CancelByAdmin;
            request.NotifyType = Notification.Error;
            request.StatusShipping = Guid.Parse(Helper.Status.CancelByAdmin);

            request.Ship.CancelByAdmin = request.UserContext.UserId;
            ChangeStatus(request);
        }



        void ChangeStatus(StatusRequest request)
        {
            var user = request.UserContext;
            var ship = request.Ship;
            var currentDate = request.CurrentDate;

            ship.IsActive = false;
            ship.ModifiedOn = currentDate;
            ship.ModifiedBy = user.UserId;
            ship.NotifyType = request.NotifyType;
            ship.NotifyText = request.Desc;
            ship.StatusShipping_StatusShippingId = request.StatusShipping;

            var tl = new TimeLine
            {
                Name = request.Title,
                Desc = request.Desc,
                CreatedBy = user.UserId,
                CreatedOn = currentDate,
                ModifiedBy = user.UserId,
                ModifiedOn = currentDate,
                TimeLineId = Guid.NewGuid(),
                IsActive = true,
                Status = request.Status,
                StatusShipping_StatusShippingId = request.StatusShipping
            };
            ship.TimeLines.Add(tl);
        }
    }
}