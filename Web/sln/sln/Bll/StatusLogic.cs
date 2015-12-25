using Michal.Project.Contract.DAL;
using Michal.Project.DataModel;
using Michal.Project.Helper;
using Michal.Project.Models;
using Michal.Project.Models.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Bll
{
    public class StatusLogic
    {
        IShippingRepository _shippingRepository;

        public StatusLogic()
        {

        }

        public StatusLogic(IShippingRepository shippingRepository)
        {
            _shippingRepository = shippingRepository;
        }

        public List<TimeLinedDetailVm> GetAllTimeLines()
        {
            List<TimeLinedDetailVm> timeLines = new List<TimeLinedDetailVm>();
            timeLines.Add(new TimeLinedDetailVm
            {
                Status = (int)TimeStatus.New,
                Desc = "חדש",
                Title = "חדש",
                ProgressBar = 1
            });
            timeLines.Add(new TimeLinedDetailVm
            {
                Status = (int)TimeStatus.ApporvallRequest,
                Desc = "אישור הזמנה",
                Title = "אישור הזמנה",
                ProgressBar = 2
            });
            timeLines.Add(new TimeLinedDetailVm
            {
                Status = (int)TimeStatus.Cancel,
                Desc = "ביטול",
                Title = "ביטול",
                ProgressBar = 3
            });
            timeLines.Add(new TimeLinedDetailVm
            {
                Status = (int)TimeStatus.Confirm,
                Desc = "מאושר ע''י חברת שליחים ",
                Title = "מאושר ע''י חברת שליחים ",
                ProgressBar = 4
            });
            timeLines.Add(new TimeLinedDetailVm
            {
                Status = (int)TimeStatus.CancelByAdmin,
                Desc = "לא מאושר ע''י חברת שליחים",
                Title = "לא מאושר ע''י חברת שליחים",
                ProgressBar = 4
            });
            
            timeLines.Add(new TimeLinedDetailVm
            {
                Status = (int)TimeStatus.Arrived,
                Desc = "השליח הגיע לקבל את החבילה מהשולח ",
                Title = "השליח הגיע לקבל את החבילה מהשולח ",
                ProgressBar = 5
            });
            timeLines.Add(new TimeLinedDetailVm
            {
                Status = (int)TimeStatus.AcceptByRunner,
                Desc = "השליח קיבל את החבילה ",
                Title = "השליח קיבל את החבילה ",
                ProgressBar = 6
            });
            timeLines.Add(new TimeLinedDetailVm
            {
                Status = (int)TimeStatus.Arrived,
                Desc = "השליח הגיע לשלוח את החבילה למקבל ",
                Title = "השליח הגיע לשלוח את החבילה למקבל ",
                ProgressBar = 7
            });
            timeLines.Add(new TimeLinedDetailVm
            {
                Status = (int)TimeStatus.AcceptByClient,
                Desc = "המשלוח התקבל- קו הסיום ",
                Title = "המשלוח התקבל- קו הסיום ",
                ProgressBar = 8
            });
            timeLines.Add(new TimeLinedDetailVm
            {
                Status = (int)TimeStatus.NoAcceptByClient,
                Desc = "המשלוח לא התקבל- קו הסיום ",
                Title = "המשלוח לא התקבל- קו הסיום ",
                ProgressBar = 8
            });
            timeLines.Add(new TimeLinedDetailVm
            {
                Status = (int)TimeStatus.Close,
                Desc = "סגירת הזמנה ",
                Title = "סגירת הזמנה ",
                ProgressBar = 8
            });

            return timeLines;
        }

        public void RemoveOrder(StatusRequestBase requestBase)
        {
            StatusRequest request = new StatusRequest(requestBase);
            var text = "המשלוח בוטל ע''י" + " " + request.UserContext.FullName;
            request.Title = text;
            request.Desc = text;
            request.Status = TimeStatus.Cancel;
            request.NotifyType = (int)AlertStyle.Error; //Notification.Error;
            request.StatusShipping = Guid.Parse(Helper.Status.Cancel);
            request.Ship.IsInProccess = false;
            request.Ship.CancelByUser = request.UserContext.UserId;
            ChangeStatus(request);
        }

        public void ApprovalRequest(StatusRequestBase requestBase)
        {
            StatusRequest request = new StatusRequest(requestBase);
            var text = "המשלוח אושר " + " " + request.Ship.Name + " " + "בתאריך " + request.CurrentDate.ToString("dd/MM/yyyy HH:mm");
            request.Title = text;
            request.Desc = text;

            request.NotifyType = (int)AlertStyle.Info; //Notification.Info;
            request.Status = TimeStatus.ApporvallRequest;
            request.StatusShipping = Guid.Parse(Helper.Status.ApporvallRequest);
            request.Ship.IsInProccess = true;
            request.Ship.ApprovalRequest = request.UserContext.UserId;
            ChangeStatus(request);
        }

        public void ConfirmRequest(StatusRequestBase requestBase, Func<string, string> GetRunner)
        {
            StatusRequest request = new StatusRequest(requestBase);
            request.ActualStartDate = DateTime.Now;
            var grantToText = General.Empty;
            if (!String.IsNullOrEmpty(request.AssignTo))
            {
                request.Ship.GrantRunner = Guid.Parse(request.AssignTo);

                grantToText = GetRunner(request.AssignTo);

            }
            else
            {
                request.Ship.GrantRunner = request.UserContext.UserId;
                grantToText = request.UserContext.FullName;
            }

            var title = "המשלוח אושר ע'' חברת השליחות" + " ע''י " + request.UserContext.FullName + " (" + request.UserContext.EmpId + ")";
            var text = title + System.Environment.NewLine + " " + "המשלוח אושר " + " " + request.Ship.Name + " " + "בתאריך " + request.CurrentDate.ToString("dd/MM/yyyy HH:mm") + " והועברה לשליח" + " " + grantToText;

            request.Title = title;
            request.Desc = text;
            request.NotifyType = (int)AlertStyle.Info;// Notification.Info;
            request.Status = TimeStatus.Confirm;
            request.StatusShipping = Guid.Parse(Helper.Status.Confirm);
            request.Ship.IsInProccess = true;
            request.Ship.ApprovalShip = request.UserContext.UserId;
            ChangeStatus(request);
        }

        public void Accept(StatusRequestBase requestBase)
        {
            StatusRequest request = new StatusRequest(requestBase);
            var user = request.UserContext;
            var ship = request.Ship;
            var title = "המשלוח  נמצא " + " ע''י השליח " + user.FullName + " (" + user.EmpId + ")";
            var text = title + System.Environment.NewLine + " " + " מספר המשלוח " + " " + ship.Name + " " + "בתאריך " + request.CurrentDate.ToString("dd/MM/yyyy HH:mm");

            request.Title = title;
            request.TimeWaitEndGet = DateTime.Now;
            request.Desc = text;
            request.NotifyType = (int)AlertStyle.Info; //Notification.Info;
            request.Status = TimeStatus.AcceptByRunner;
            request.StatusShipping = Guid.Parse(Helper.Status.AcceptByRunner);
            request.Ship.IsInProccess = true;
            request.Ship.BroughtShipmentCustomer = request.UserContext.UserId;

            ChangeStatus(request);
        }

        public void CancelRequest(StatusRequestBase requestBase)
        {
            StatusRequest request = new StatusRequest(requestBase);
            var user = request.UserContext;
            var ship = request.Ship;
            var currentDate = request.CurrentDate;
            var text = "המשלוח לא מאושר" + " " + ship.Name + " " + "בתאריך " + currentDate.ToString("dd/MM/yyyy HH:mm");
            request.Title = text;
            request.Desc = text;
            request.Status = TimeStatus.CancelByAdmin;
            request.NotifyType = (int)AlertStyle.Error; //Notification.Error;
            request.StatusShipping = Guid.Parse(Helper.Status.CancelByAdmin);
            request.Ship.IsInProccess = false;
            request.Ship.CancelByAdmin = request.UserContext.UserId;
            ChangeStatus(request);
        }

        public void ArrivedGet(StatusRequestBase requestBase)
        {
            var request = new StatusRequest(requestBase);
            var user = request.UserContext;
            var ship = request.Ship;
            request.TimeWaitStartGet = DateTime.Now;

            var title = "המשלוח  הגיע לקבל   " + " ע''י השליח " + user.FullName + " (" + user.EmpId + ")";
            var text = title + System.Environment.NewLine + " " + " מספר משלוח " + " " + ship.Name + " " + "בתאריך " + request.CurrentDate.ToString("dd/MM/yyyy HH:mm");

            request.Title = title;
            request.Desc = text;
            request.NotifyType = (int)AlertStyle.Info; //Notification.Info;
            request.Status = TimeStatus.ArrivedSender;
            request.StatusShipping = Guid.Parse(Helper.Status.ArrivedSender);
            request.Ship.IsInProccess = true;
            request.Ship.ArrivedShippingGet = request.UserContext.UserId;
            ChangeStatus(request);
        }

        public void Arrived(StatusRequestBase requestBase)
        {
            var request = new StatusRequest(requestBase);
            var user = request.UserContext;
            var ship = request.Ship;
            request.TimeWaitStartSend = DateTime.Now;
            var title = "המשלוח  הגיע  " + " ע''י השליח " + user.FullName + " (" + user.EmpId + ")";
            var text = title + System.Environment.NewLine + " " + " מספר משלוח " + " " + ship.Name + " " + "בתאריך " + request.CurrentDate.ToString("dd/MM/yyyy HH:mm");

            request.Title = title;
            request.Desc = text;
            request.NotifyType = (int)AlertStyle.Info; //Notification.Info;
            request.Status = TimeStatus.Arrived;
            request.StatusShipping = Guid.Parse(Helper.Status.Arrived);
            request.Ship.IsInProccess = true;
            request.Ship.ArrivedShippingSender = request.UserContext.UserId;
            ChangeStatus(request);
        }

        public void Take(StatusRequestBase requestBase, string desc, string recipient)
        {
            var request = new StatusRequest(requestBase);
            var user = request.UserContext;
            var ship = request.Ship;
            var title = "המשלוח  התקבל  " + " ע''י השליח " + user.FullName + " (" + user.EmpId + ")" + " " + " מספר משלוח " + " " + ship.Name + " " + "בתאריך " + request.CurrentDate.ToString("dd/MM/yyyy HH:mm");
            var text = title + desc;

            ship.ActualRecipient = recipient;

            request.Title = title;
            request.TimeWaitEndSend = DateTime.Now;
            request.Desc = text;
            request.NotifyType = (int)AlertStyle.Success; //Notification.Success;//Notification.Success;
            request.Status = TimeStatus.AcceptByClient;
            request.StatusShipping = Guid.Parse(Helper.Status.AcceptByClient);
            request.Ship.IsInProccess = true;
            request.Ship.BroughtShipmentCustomer = request.UserContext.UserId;
            ChangeStatus(request);
        }

        public void NoTake(StatusRequestBase requestBase, string desc)
        {
            var request = new StatusRequest(requestBase);
            var user = request.UserContext;
            var ship = request.Ship;
            var title = "המשלוח  לא   " + " ע''י השליח " + user.FullName + " (" + user.EmpId + ")" + " " + " מספר משלוח " + " " + ship.Name + " " + "בתאריך " + request.CurrentDate.ToString("dd/MM/yyyy HH:mm");
            var text = title + desc;
            request.TimeWaitEndSend = DateTime.Now;

            request.Title = title;
            request.Desc = text;
            request.NotifyType = (int)AlertStyle.Error; //Notification.Error;
            request.StatusShipping = Guid.Parse(Helper.Status.NoAcceptByClient);
            request.Ship.IsInProccess = false;
            request.Ship.NoBroughtShipmentCustomer = request.UserContext.UserId;
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
            if (!String.IsNullOrEmpty(request.EndDesc))
                ship.EndDesc = request.EndDesc;

            if (request.TimeWaitStartGet.HasValue)
                ship.TimeWaitStartSGet = request.TimeWaitStartGet.Value;
            if (request.TimeWaitEndGet.HasValue)
                ship.TimeWaitEndGet = request.TimeWaitEndGet.Value;

            if (request.TimeWaitStartSend.HasValue)
                ship.TimeWaitStartSend = request.TimeWaitStartSend.Value;
            if (request.TimeWaitEndSend.HasValue)
                ship.TimeWaitEndSend = request.TimeWaitEndSend.Value;

            if (request.ActualStartDate.HasValue)
                ship.ActualStartDate = request.ActualStartDate.Value;
            if (request.ActualEndDate.HasValue)
                ship.ActualEndDate = request.ActualEndDate.Value;


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

        public void ConfirmRequest(StatusRequestBase requestBase)
        {
            StatusRequest request = new StatusRequest(requestBase);
            request.ActualStartDate = DateTime.Now;
            var grantToText = General.Empty;

            request.Ship.GrantRunner = request.UserContext.UserId;
            grantToText = request.UserContext.FullName;


            var title = "המשלוח אושר ע'' חברת השליחות" + " ע''י " + request.UserContext.FullName + " (" + request.UserContext.EmpId + ")";
            var text = title + System.Environment.NewLine + " " + "המשלוח אושר " + " " + request.Ship.Name + " " + "בתאריך " + request.CurrentDate.ToString("dd/MM/yyyy HH:mm") + " והועברה לשליח" + " " + grantToText;

            request.Title = title;
            request.Desc = text;
            request.NotifyType = (int)AlertStyle.Info;// Notification.Info;
            request.Status = TimeStatus.Confirm;
            request.StatusShipping = Guid.Parse(Helper.Status.Confirm);
            request.Ship.IsInProccess = true;
            request.Ship.ApprovalShip = request.UserContext.UserId;
            ChangeStatus(request);
        }

        public void ConfirmRequest2(StatusRequestBase requestBase)
        {
            StatusRequest request = new StatusRequest(requestBase);
            request.ActualStartDate = DateTime.Now;
            var grantToText = General.Empty;

            request.Ship.GrantRunner = request.UserContext.UserId;
            grantToText = request.UserContext.FullName;


            var title = "המשלוח אושר ע'' חברת השליחות" + " ע''י " + request.UserContext.FullName + " (" + request.UserContext.EmpId + ")";
            var text = title + System.Environment.NewLine + " " + "המשלוח אושר " + " " + request.Ship.Name + " " + "בתאריך " + request.CurrentDate.ToString("dd/MM/yyyy HH:mm") + " והועברה לשליח" + " " + grantToText;

            request.Title = title;
            request.Desc = text;
            request.NotifyType = (int)AlertStyle.Info;// Notification.Info;
            request.Status = TimeStatus.Confirm;
            request.StatusShipping = Guid.Parse(Helper.Status.Confirm);
            request.Ship.IsInProccess = true;
            request.Ship.ApprovalShip = request.UserContext.UserId;

            ChangeStatus(request);
            if (_shippingRepository != null)
                _shippingRepository.Update(request.Ship);

        }

        public void ApprovalRequest2(StatusRequestBase requestBase)
        {
            StatusRequest request = new StatusRequest(requestBase);
            var text = "המשלוח אושרה " + " " + request.Ship.Name + " " + "בתאריך " + request.CurrentDate.ToString("dd/MM/yyyy HH:mm");
            request.Title = text;
            request.Desc = text;

            request.NotifyType = (int)AlertStyle.Info; //Notification.Info;
            request.Status = TimeStatus.ApporvallRequest;
            request.StatusShipping = Guid.Parse(Helper.Status.ApporvallRequest);
            request.Ship.IsInProccess = true;

            request.Ship.ApprovalRequest = request.UserContext.UserId;
            ChangeStatus(request);
            if (_shippingRepository != null)
                _shippingRepository.Update(request.Ship);
        }

        public void RemoveOrder2(StatusRequestBase requestBase)
        {
            StatusRequest request = new StatusRequest(requestBase);
            var text = "המשלוח בוטל ע''י" + " " + request.UserContext.FullName;
            request.Title = text;
            request.Desc = text;
            request.Status = TimeStatus.Cancel;
            request.NotifyType = (int)AlertStyle.Error; //Notification.Error;
            request.StatusShipping = Guid.Parse(Helper.Status.Cancel);
            request.Ship.IsInProccess = false;

            request.Ship.CancelByUser = request.UserContext.UserId;
            ChangeStatus(request);
            if (_shippingRepository != null)
                _shippingRepository.Update(request.Ship);
        }

        public void CancelRequest2(StatusRequestBase requestBase)
        {
            StatusRequest request = new StatusRequest(requestBase);
            var user = request.UserContext;
            var ship = request.Ship;
            var currentDate = request.CurrentDate;
            var text = "המשלוח לא מאושר" + " " + ship.Name + " " + "בתאריך " + currentDate.ToString("dd/MM/yyyy HH:mm");
            request.Title = text;
            request.Desc = text;
            request.Status = TimeStatus.CancelByAdmin;
            request.NotifyType = (int)AlertStyle.Error; //Notification.Error;
            request.StatusShipping = Guid.Parse(Helper.Status.CancelByAdmin);
            request.Ship.IsInProccess = false;

            request.Ship.CancelByAdmin = request.UserContext.UserId;
            ChangeStatus(request);
            if (_shippingRepository != null)
                _shippingRepository.Update(request.Ship);
        }

        public void Close(StatusRequestBase requestBase)
        {
            StatusRequest request = new StatusRequest(requestBase);
            var user = request.UserContext;
            var ship = request.Ship;
            var currentDate = request.CurrentDate;
            var text = "המשלוח נסגר" + " " + ship.Name + " מחיר בפועל הינו " + ship.ActualPrice.ToString() + " " + General.Shekel + " בתאריך " + currentDate.ToString("dd/MM/yyyy HH:mm");
            request.Title = text;
            request.Desc = text;
            request.Status = TimeStatus.Close;
            request.NotifyType = (int)AlertStyle.Success; //Notification.Error;
            request.StatusShipping = Guid.Parse(Helper.Status.Close);
            request.Ship.IsInProccess = false;

            request.Ship.ClosedShippment = request.UserContext.UserId;
            ChangeStatus(request);
            if (_shippingRepository != null)
                _shippingRepository.Update(request.Ship);
        }

    }
}