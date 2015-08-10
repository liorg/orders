using sln.Contract;
using sln.DataModel;
using sln.Helper;
using sln.Models;
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

namespace sln.Bll
{
    
    public class ViewLogic
    {
        public void SetViewerUserByRole(sln.Contract.IRole source, IViewerUser target)
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

        //public async Task<ViewListsResponse> Get(ViewListsRequest request)
        //{
        //    ViewListsResponse response = new ViewListsResponse();
        //    var orgId=Guid.Empty;
        //    int order = request.ViewType.HasValue ? request.ViewType.Value : request.UserContext.DefaultView;
        //    if (request.ViewType.HasValue)
        //    {
        //        var view = request.View.Where(g => g.StatusId == request.ViewType.Value).FirstOrDefault();
        //        if (view != null)
        //            response.StatusDesc = view.StatusDesc;
        //    }
        //    List<Shipping> shippings = new List<Shipping>();
        //    var from = DateTime.Today.AddDays(-1);
        //    var shippingsQuery = request.shippingsQuery;//context.Shipping.Where(s => s.StatusShipping.OrderDirection == order && s.CreatedOn > from).AsQueryable();
        //    if (!request.User.IsInRole(HelperAutorize.RoleAdmin))
        //    {
        //       orgId=request.UserContext.OrgId; 
        //    }
        //    shippings =  await shippingsQuery.Where(sx => sx.Organization_OrgId.HasValue && (sx.Organization_OrgId.Value == orgId || orgId == Guid.Empty)).ToListAsync();
        //    var model = new List<ShippingVm>();
        //    foreach (var ship in shippings)
        //    {
        //        var created = "";//context.Users.Find(ship.CreatedBy.ToString());

        //        var u = new ShippingVm();
        //        u.Id = ship.ShippingId;
        //        u.Status = ship.StatusShipping.Desc;
        //        u.Name = ship.Name;
        //        u.DistanceName = ship.Distance != null ? ship.Distance.Name : "";
        //       // u.CreatedBy = created != null ? created.FirstName + " " + created.LastName : "";
        //        u.CityToName = ship.CityTo != null ? ship.CityTo.Name : "";
        //        u.CityFormName = ship.CityFrom != null ? ship.CityFrom.Name : "";
        //        u.CreatedOn = ship.CreatedOn.HasValue ? ship.CreatedOn.Value.ToString("dd/MM/yyyy hh:mm") : "";
        //        model.Add(u);

        //    }
        //    return response;
        //}

        public OrderView GetOrder(OrderRequest request)
        {

            var orderModel = new OrderView();
            var shipping = request.Shipping;
            var runners = request.Runners;
            orderModel.Status = new StatusVm();
            orderModel.Status.StatusId = shipping.StatusShipping_StatusShippingId.GetValueOrDefault();
            orderModel.Status.Recipient = shipping.Recipient;
            orderModel.Status.Name = shipping.StatusShipping != null ? shipping.StatusShipping.Desc : "";
            orderModel.Status.MessageType = shipping.NotifyType; //Notification.Warning; //Notification.Error;//Notification.Warning;
            orderModel.Status.Message = shipping.NotifyText;
            orderModel.Status.ShipId = shipping.ShippingId;
            orderModel.Status.Runners = runners;
            orderModel.ShippingVm = new ShippingVm();
            orderModel.ShippingVm.Number = shipping.Name;
            orderModel.ShippingVm.CityForm = shipping.CityFrom_CityId.GetValueOrDefault();
            orderModel.ShippingVm.CityTo = shipping.CityTo_CityId.GetValueOrDefault();
            orderModel.ShippingVm.DistanceId = shipping.Distance_DistanceId.GetValueOrDefault();
            orderModel.ShippingVm.FastSearch = shipping.FastSearchNumber;
            orderModel.ShippingVm.Id = shipping.ShippingId;
            orderModel.ShippingVm.Number = shipping.Desc;
            orderModel.ShippingVm.NumFrom = shipping.AddressNumFrom;
            orderModel.ShippingVm.NumTo = shipping.AddressNumTo;
            orderModel.ShippingVm.OrgId = shipping.Organization_OrgId.GetValueOrDefault();
            orderModel.ShippingVm.SreetFrom = shipping.AddressFrom;
            orderModel.ShippingVm.SreetTo = shipping.AddressTo;
            orderModel.ShippingVm.Status = shipping.StatusShipping != null ? shipping.StatusShipping.Desc : "";
            orderModel.ShippingVm.StatusId = shipping.StatusShipping_StatusShippingId.GetValueOrDefault();
            orderModel.ShippingVm.OrgId = shipping.Organization_OrgId.GetValueOrDefault();
            orderModel.ShippingVm.CreatedOn = shipping.CreatedOn.Value.ToString("dd/MM/yyyy");
            orderModel.ShippingVm.ModifiedOn = shipping.ModifiedOn.Value.ToString("dd/MM/yyyy");

            orderModel.ShippingVm.TelSource = shipping.TelSource;
            orderModel.ShippingVm.TelTarget = shipping.TelTarget;
            orderModel.ShippingVm.NameSource = shipping.NameSource;
            orderModel.ShippingVm.NameTarget = shipping.NameTarget;
            orderModel.ShippingVm.Recipient = shipping.Recipient;

            orderModel.ShippingVm.CityFormName = shipping.CityFrom != null ? shipping.CityFrom.Name : "";
            orderModel.ShippingVm.CityToName = shipping.CityTo != null ? shipping.CityTo.Name : "";
            orderModel.IsEyeOnHim = shipping.FollowsBy.Where(fx => fx.Id == request.UserContext.UserId.ToString()).Any();

            orderModel.JobTitle = request.UserContext;

            var timeLineVms = new List<TimeLineVm>();
            foreach (var timeline in shipping.TimeLines.OrderByDescending(t => t.CreatedOn))
            {
                timeLineVms.Add(new TimeLineVm { Title = timeline.Name, CreatedOn = timeline.CreatedOn.GetValueOrDefault(), TimeLineId = timeline.TimeLineId, Desc = timeline.Desc, Status = timeline.Status });
            }
            var comments = new List<CommentVm>();
            foreach (var comment in shipping.Comments.OrderByDescending(t => t.CreatedOn))
            {
                comments.Add(new CommentVm {Name=comment.Name, JobTitle = comment.JobTitle,JobType=comment.JobType, CreatedOn = comment.CreatedOn.GetValueOrDefault(),  Desc = comment.Desc });
            }
            orderModel.TimeLineVms = timeLineVms;
            orderModel.CommentsVm = comments;
            return orderModel;
        }

        //public void SetJob(ClaimsIdentity identity, IEnumerable<Claim> claims)
        //{
        //    var claimAllRoles = claims.Where(ccl => ccl.Type == ClaimTypes.Role).AsEnumerable();
        //    foreach (var claimRole in claimAllRoles)
        //    {
        //        if (claimRole != null && !String.IsNullOrWhiteSpace(claimRole.Value))
        //        {
        //            identity.
        //        }
        //    }
        //    if (user.IsInRole(Helper.HelperAutorize.RoleAdmin))
        //    {
        //        job.JobTitle = Helper.JobTitle.Admin;
        //        job.JobType = ((int)Helper.JobType.Admin).ToString();
        //        return;
        //    }
        //    if (user.IsInRole(Helper.HelperAutorize.RoleRunner))
        //    {
        //        job.JobTitle = Helper.JobTitle.DeliveryBoy;
        //        job.JobType = ((int)Helper.JobType.Runner).ToString();
        //        return;
        //    }
        //    job.JobTitle = Helper.JobTitle.Client;
        //    job.JobType = ((int)Helper.JobType.Client).ToString();
        //}

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