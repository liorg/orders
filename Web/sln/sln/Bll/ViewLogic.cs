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

namespace Michal.Project.Bll
{

    public class ViewLogic
    {

        public ViewLogic()
        {

        }
        readonly IShippingRepository _shippingRepository;
        readonly IUserRepository _userRepository;
        readonly IOrgDetailRepostory _orgDetailRepostory;

        public ViewLogic(IShippingRepository shippingRepository, IUserRepository userRepository, IOrgDetailRepostory orgDetailRepostory)
        {
            _shippingRepository = shippingRepository;
            _userRepository = userRepository;
            _orgDetailRepostory = orgDetailRepostory;

        }
        public void SetViewerUserByRole(Michal.Project.Contract.IRole source, IViewerUser target)
        {
            bool showAll = true;
            int defaultView = TimeStatus.New;
            if (source.IsCreateOrder)
                showAll = false;
            if (source.IsAcceptOrder)
            {
                showAll = false;
                defaultView = TimeStatus.ApporvallRequest;
            }
            if (source.IsOrgMangager)
            {
                showAll = true;
                defaultView = TimeStatus.AcceptByRunner;
            }
            if (source.IsRunner)
            {
                showAll = false;
                defaultView = TimeStatus.AcceptByRunner;
            }
            if (source.IsAdmin)
            {
                showAll = false;
                defaultView = TimeStatus.Confirm;
            }
            target.DefaultView = defaultView;
            target.ViewAll = showAll;


        }

        public OrderViewStatus GetOrderStatus(AttachmentShipping sign, OrderRequest request)
        {

            var orderModel = new OrderViewStatus();
            var shipping = request.Shipping;
            //var runners = request.Runners;

            orderModel.Status = new StatusVm();
            if (sign != null)
                orderModel.Status.PathSig = sign.Path;

            orderModel.Status.StatusId = shipping.StatusShipping_StatusShippingId.GetValueOrDefault();
            orderModel.Status.Recipient = shipping.Direction == 0 ? shipping.Recipient : shipping.NameSource;
            orderModel.Status.TelRecipient = shipping.Direction == 0 ? shipping.TelTarget : shipping.TelSource;
            orderModel.Status.NameTarget = shipping.Direction == 0 ? shipping.NameTarget : shipping.NameSource;
            orderModel.Status.NameActualRecipient = shipping.ActualRecipient != null ? shipping.ActualRecipient : General.Empty;
            orderModel.Status.NameActualTelRecipient = shipping.ActualTelTarget != null ? shipping.ActualTelTarget : General.Empty; //shipping.ActualTelTarget;
            orderModel.Status.NameActualTarget = shipping.ActualNameTarget != null ? shipping.ActualNameTarget : General.Empty; // shipping.ActualNameTarget;
            orderModel.Status.Name = shipping.StatusShipping != null ? shipping.StatusShipping.Desc : General.Empty;
            orderModel.Status.MessageType = (AlertStyle)shipping.NotifyType; //Notification.Warning; //Notification.Error;//Notification.Warning;
            orderModel.Status.Message = shipping.NotifyText;
            orderModel.Status.ShipId = shipping.ShippingId;
            orderModel.Status.IsTake = shipping.StatusShipping_StatusShippingId.HasValue && shipping.StatusShipping_StatusShippingId == Guid.Parse(Helper.Status.AcceptByClient);
            orderModel.Status.Desc = !String.IsNullOrEmpty(shipping.EndDesc) ? shipping.EndDesc : General.Empty;
            orderModel.Status.SigBackType = shipping.SigBackType.GetValueOrDefault();
            //orderModel.Status.Runners = runners;
            orderModel.Status.ActualEndDate = shipping.ActualEndDate.HasValue ? shipping.ActualEndDate.Value.ToString("dd-MM-yyyy HH:mm") : General.Empty; 
            orderModel.Status.ActualStartDate = shipping.ActualStartDate.HasValue ? shipping.ActualStartDate.Value.ToString("dd-MM-yyyy HH:mm") : General.Empty;

            orderModel.Location = new Location();
            orderModel.Location.TargetLat = shipping.Target.Lat;
            orderModel.Location.TargetLng = shipping.Target.Lng;
            orderModel.Location.SourceLat = shipping.Source.Lat;
            orderModel.Location.SourceLng = shipping.Source.Lng;

            orderModel.ShippingVm = new ShippingVm();
            orderModel.ShippingVm.Name = shipping.Name;
            orderModel.ShippingVm.Direction = shipping.Direction;
            orderModel.ShippingVm.DistanceId = shipping.Distance_DistanceId.GetValueOrDefault();
            orderModel.ShippingVm.FastSearch = shipping.FastSearchNumber;
            orderModel.ShippingVm.Id = shipping.ShippingId;
            orderModel.ShippingVm.Number = shipping.Desc;
            orderModel.ShippingVm.OrgId = shipping.Organization_OrgId.GetValueOrDefault();

            orderModel.ShippingVm.Status = shipping.StatusShipping != null ? shipping.StatusShipping.Desc : General.Empty;

            orderModel.ShippingVm.StatusPresent = shipping.StatusShipping.OrderDirection == 0 ? 0 : (double)(shipping.StatusShipping.OrderDirection / (double)Status.Max) * 100;

            orderModel.ShippingVm.StatusId = shipping.StatusShipping_StatusShippingId.GetValueOrDefault();
            orderModel.ShippingVm.OrgId = shipping.Organization_OrgId.GetValueOrDefault();
            orderModel.ShippingVm.CreatedOn = shipping.CreatedOn.Value.ToString("dd/MM/yyyy");
            orderModel.ShippingVm.ModifiedOn = shipping.ModifiedOn.Value.ToString("dd/MM/yyyy");

            orderModel.ShippingVm.TelSource = shipping.TelSource != null ? shipping.TelSource : General.Empty;
            orderModel.ShippingVm.TelTarget = shipping.TelTarget != null ? shipping.TelTarget : General.Empty; //shipping.TelTarget;
            orderModel.ShippingVm.NameSource = shipping.NameSource != null ? shipping.NameSource : General.Empty;// shipping.NameSource;
            orderModel.ShippingVm.NameTarget = shipping.NameTarget != null ? shipping.NameTarget : General.Empty;//  shipping.NameTarget;
            orderModel.ShippingVm.Recipient = shipping.Recipient != null ? shipping.Recipient : General.Empty;//  shipping.Recipient;

            orderModel.ShippingVm.SourceAddress = new AddressEditorViewModel();
            orderModel.ShippingVm.SourceAddress.City = shipping.Source.CityName;
            orderModel.ShippingVm.SourceAddress.Citycode = shipping.Source.CityCode;
            orderModel.ShippingVm.SourceAddress.CitycodeOld = shipping.Source.CityCode;
            orderModel.ShippingVm.SourceAddress.Street = shipping.Source.StreetName;
            orderModel.ShippingVm.SourceAddress.Streetcode = shipping.Source.StreetCode;
            orderModel.ShippingVm.SourceAddress.StreetcodeOld = shipping.Source.StreetCode;
            orderModel.ShippingVm.SourceAddress.ExtraDetail = shipping.Source.ExtraDetail;
            orderModel.ShippingVm.SourceAddress.Num = shipping.Source.StreetNum;

            orderModel.ShippingVm.TargetAddress = new AddressEditorViewModel();
            orderModel.ShippingVm.TargetAddress.City = shipping.Target.CityName;
            orderModel.ShippingVm.TargetAddress.Citycode = shipping.Target.CityCode;
            orderModel.ShippingVm.TargetAddress.CitycodeOld = shipping.Target.CityCode;
            orderModel.ShippingVm.TargetAddress.Street = shipping.Target.StreetName;
            orderModel.ShippingVm.TargetAddress.Streetcode = shipping.Target.StreetCode;
            orderModel.ShippingVm.TargetAddress.StreetcodeOld = shipping.Target.StreetCode;
            orderModel.ShippingVm.TargetAddress.ExtraDetail = shipping.Target.ExtraDetail;
            orderModel.ShippingVm.TargetAddress.Num = shipping.Target.StreetNum;

            return orderModel;
        }

        public async Task<RunnerView> GetUser(Guid shipId)
        {
            var orderModel = new RunnerView();
            orderModel.Runners = new List<Runner>();
            orderModel.CurrentRunner = new UserDetail();
            var orgid = _orgDetailRepostory.GetOrg();

            var shipping = await _shippingRepository.GetShip(shipId);
            if (shipping == null) throw new ArgumentNullException("shipping");
            orderModel.CurrentRunner = await _userRepository.GetUser(shipping.GrantRunner);
            var company = _orgDetailRepostory.GetShippingCompaniesByOrgId(orgid).FirstOrDefault();

            if (company != null && company.Users != null && company.Users.Any())
            {
                foreach (var user in company.Users)
                {
                    //if (user.Id == orderModel.CurrentRunner.UserId)
                    //    continue;
                    orderModel.Runners.Add(new Runner { FirstName = user.FirstName, Id = user.Id, Lastname = user.LastName });
                }
            }


            orderModel.Id = shipping.ShippingId;
            orderModel.Name = shipping.Name;
            orderModel.ShippingVm = new ShippingVm();
            orderModel.ShippingVm.Id = orderModel.Id;
            orderModel.ShippingVm.Name = orderModel.Name;

            return orderModel;
        }

        public async Task<RunnerDetail> GetShippingByUser(Guid userid)
        {
            var runnerModel = new RunnerDetail();
            runnerModel.Id = userid;
            var user = await _userRepository.GetUser(userid);
            runnerModel.Name = user.FullName;

            var shipping = await _shippingRepository.GetShippingByUserId(userid);
            if (shipping == null) throw new ArgumentNullException("shipping");
            runnerModel.Items = shipping;

            return runnerModel;
        }

        public async Task<OrderView> GetTimeLine(Guid shipId)
        {
            var orderModel = new OrderView();

            var shipping = await _shippingRepository.GetShipTimelines(shipId);//request.Shipping;
            orderModel.Id = shipping.ShippingId;
            orderModel.Name = shipping.Name;
            orderModel.ShippingVm = new ShippingVm();
            orderModel.ShippingVm.Id = orderModel.Id;
            orderModel.ShippingVm.Name = orderModel.Name;
            orderModel.ShippingVm.StatusPresent = shipping.StatusShipping.OrderDirection == 0 ? 0 : (double)(shipping.StatusShipping.OrderDirection / (double)Status.Max) * 100;

            var timeLineVms = new List<TimeLineVm>();
            foreach (var timeline in shipping.TimeLines.OrderByDescending(t => t.CreatedOn))
            {
                timeLineVms.Add(new TimeLineVm { Title = timeline.Name, CreatedOn = timeline.CreatedOn.GetValueOrDefault(), TimeLineId = timeline.TimeLineId, Desc = timeline.Desc, Status = timeline.Status });
            }

            orderModel.TimeLineVms = timeLineVms;

            return orderModel;
        }

        //public OrderView GetTimeLine(OrderRequest request)
        //{
        //    var orderModel = new OrderView();

        //    var shipping = request.Shipping;
        //    orderModel.Id = shipping.ShippingId;
        //    orderModel.Name = shipping.Name;
        //    orderModel.ShippingVm = new ShippingVm();
        //    orderModel.ShippingVm.Id = orderModel.Id;
        //    orderModel.ShippingVm.Name = orderModel.Name;
        //    orderModel.ShippingVm.StatusPresent = shipping.StatusShipping.OrderDirection == 0 ? 0 : (double)(shipping.StatusShipping.OrderDirection / (double)Status.Max) * 100;

        //    var timeLineVms = new List<TimeLineVm>();
        //    foreach (var timeline in shipping.TimeLines.OrderByDescending(t => t.CreatedOn))
        //    {
        //        timeLineVms.Add(new TimeLineVm { Title = timeline.Name, CreatedOn = timeline.CreatedOn.GetValueOrDefault(), TimeLineId = timeline.TimeLineId, Desc = timeline.Desc, Status = timeline.Status });
        //    }

        //    orderModel.TimeLineVms = timeLineVms;

        //    return orderModel;
        //}

        public OrderView GetOrder(OrderRequest request)
        {

            var orderModel = new OrderView();

            var shipping = request.Shipping;
            orderModel.Id = shipping.ShippingId;
            orderModel.Name = shipping.Name;
            // var runners = request.Runners;
            orderModel.Status = new StatusVm();
            orderModel.Status.StatusId = shipping.StatusShipping_StatusShippingId.GetValueOrDefault();
            orderModel.Status.Recipient = shipping.Recipient;

            orderModel.Status.Name = shipping.StatusShipping != null ? shipping.StatusShipping.Desc : General.Empty;
            orderModel.Status.MessageType = (AlertStyle)shipping.NotifyType; //Notification.Warning; //Notification.Error;//Notification.Warning;
            orderModel.Status.Message = shipping.NotifyText;
            orderModel.Status.ShipId = shipping.ShippingId;
            //  orderModel.Status.Runners = runners;
            orderModel.Location = new Location();
            orderModel.Location.TargetLat = shipping.Target.Lat;
            orderModel.Location.TargetLng = shipping.Target.Lng;

            orderModel.Location.SourceLat = shipping.Source.Lat;
            orderModel.Location.SourceLng = shipping.Source.Lng;


            orderModel.ShippingVm = new ShippingVm();
            orderModel.ShippingVm.Direction = shipping.Direction;
            orderModel.ShippingVm.Name = shipping.Name;

            orderModel.ShippingVm.DistanceId = shipping.Distance_DistanceId.GetValueOrDefault();
            orderModel.ShippingVm.DistanceName = shipping.Distance.Name;
            orderModel.ShippingVm.ShipTypeIdName = shipping.ShipType.Name;
            orderModel.ShippingVm.FastSearch = shipping.FastSearchNumber;
            orderModel.ShippingVm.Id = shipping.ShippingId;
            orderModel.ShippingVm.Number = shipping.Desc;
            orderModel.ShippingVm.OrgId = shipping.Organization_OrgId.GetValueOrDefault();

            orderModel.ShippingVm.Status = shipping.StatusShipping != null ? shipping.StatusShipping.Desc : General.Empty;
            orderModel.ShippingVm.StatusId = shipping.StatusShipping_StatusShippingId.GetValueOrDefault();
            orderModel.ShippingVm.OrgId = shipping.Organization_OrgId.GetValueOrDefault();
            orderModel.ShippingVm.CreatedOn = shipping.CreatedOn.Value.ToString("dd/MM/yyyy HH:mm");
            orderModel.ShippingVm.ModifiedOn = shipping.ModifiedOn.Value.ToString("dd/MM/yyyy HH:mm");
            orderModel.ShippingVm.ActualStartDate = shipping.ActualStartDate.HasValue ? shipping.ActualStartDate.Value.ToString("dd/MM/yyyy HH:mm") : General.Empty;
            orderModel.ShippingVm.SlaDate = shipping.SlaTime.HasValue ? shipping.SlaTime.Value == DateTime.Now.Date ? shipping.SlaTime.Value.ToString("HH:mm") : shipping.SlaTime.Value.ToString("dd/MM/yyyy HH:mm") : General.Empty;
            orderModel.ShippingVm.StatusPresent = shipping.StatusShipping.OrderDirection == 0 ? 0 : (double)(shipping.StatusShipping.OrderDirection / (double)Status.Max) * 100;

            orderModel.ShippingVm.TelSource = shipping.TelSource;
            orderModel.ShippingVm.TelTarget = shipping.TelTarget;
            orderModel.ShippingVm.NameSource = shipping.NameSource;
            orderModel.ShippingVm.NameTarget = shipping.NameTarget;
            orderModel.ShippingVm.Recipient = shipping.Recipient;

            orderModel.ShippingVm.SourceAddress = new AddressEditorViewModel();
            orderModel.ShippingVm.SourceAddress.City = shipping.Source.CityName;
            orderModel.ShippingVm.SourceAddress.Citycode = shipping.Source.CityCode;
            orderModel.ShippingVm.SourceAddress.CitycodeOld = shipping.Source.CityCode;
            orderModel.ShippingVm.SourceAddress.Street = shipping.Source.StreetName;
            orderModel.ShippingVm.SourceAddress.Streetcode = shipping.Source.StreetCode;
            orderModel.ShippingVm.SourceAddress.StreetcodeOld = shipping.Source.StreetCode;
            orderModel.ShippingVm.SourceAddress.ExtraDetail = shipping.Source.ExtraDetail;
            orderModel.ShippingVm.SourceAddress.Num = shipping.Source.StreetNum;


            orderModel.ShippingVm.TargetAddress = new AddressEditorViewModel();
            orderModel.ShippingVm.TargetAddress.City = shipping.Target.CityName;
            orderModel.ShippingVm.TargetAddress.Citycode = shipping.Target.CityCode;
            orderModel.ShippingVm.TargetAddress.CitycodeOld = shipping.Target.CityCode;
            orderModel.ShippingVm.TargetAddress.Street = shipping.Target.StreetName;
            orderModel.ShippingVm.TargetAddress.Streetcode = shipping.Target.StreetCode;
            orderModel.ShippingVm.TargetAddress.StreetcodeOld = shipping.Target.StreetCode;
            orderModel.ShippingVm.TargetAddress.ExtraDetail = shipping.Target.ExtraDetail;
            orderModel.ShippingVm.TargetAddress.Num = shipping.Target.StreetNum;

            orderModel.IsEyeOnHim = shipping.FollowsBy.Where(fx => fx.Id == request.UserContext.UserId.ToString()).Any();

            orderModel.JobTitle = request.UserContext;

            //var timeLineVms = new List<TimeLineVm>();
            //foreach (var timeline in shipping.TimeLines.OrderByDescending(t => t.CreatedOn))
            //{
            //    timeLineVms.Add(new TimeLineVm { Title = timeline.Name, CreatedOn = timeline.CreatedOn.GetValueOrDefault(), TimeLineId = timeline.TimeLineId, Desc = timeline.Desc, Status = timeline.Status });
            //}
            var comments = new List<CommentVm>();
            if (shipping.Comments != null && shipping.Comments.Any())
            {
                foreach (var comment in shipping.Comments.OrderByDescending(t => t.CreatedOn))
                {
                    comments.Add(new CommentVm { Name = comment.Name, JobTitle = comment.JobTitle, JobType = comment.JobType, CreatedOn = comment.CreatedOn.GetValueOrDefault(), Desc = comment.Desc });
                }
            }
            // orderModel.TimeLineVms = timeLineVms;
            orderModel.CommentsVm = comments;
            return orderModel;
        }

        public void SetJob(IJob job, UserContext user)
        {

            job.JobTitle = user.JobTitle;
            job.JobType = user.JobType;
        }

        public void SetJob(IJob job, IPrincipal user)
        {
            if (user.IsInRole(Helper.HelperAutorize.RoleAdmin))
            {
                job.JobTitle = Helper.JobTitle.Admin;
                job.JobType = ((int)Helper.JobType.Admin).ToString();
                return;
            }
            if (user.IsInRole(Helper.HelperAutorize.RoleRunner))
            {
                job.JobTitle = Helper.JobTitle.DeliveryBoy;
                job.JobType = ((int)Helper.JobType.Runner).ToString();
                return;
            }
            job.JobTitle = Helper.JobTitle.Client;
            job.JobType = ((int)Helper.JobType.Client).ToString();
        }
    }
}