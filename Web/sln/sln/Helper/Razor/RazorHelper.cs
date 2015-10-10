using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
namespace Michal.Project.Helper
{
    public static class RazorHelper
    {
        public static MvcHtmlString DisplayColumnNameFor<TModel, TClass, TProperty>(
    this HtmlHelper<TModel> helper, IEnumerable<TClass> model,
    Expression<Func<TClass, TProperty>> expression)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(
                () => Activator.CreateInstance<TClass>(), typeof(TClass), name);

            return new MvcHtmlString(metadata.DisplayName);
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string jobType)
        {
            var builder = new TagBuilder("img");
            var src = "/Content/img/Male-Avatar-Emo-Haircut-icon.png";
            JobType eJob = JobType.Client;
            if (!String.IsNullOrWhiteSpace(jobType))
            {
                int ijob = int.Parse(jobType);
                eJob = (JobType)ijob;
                ///eJob = (JobTitle)Enum.Parse(JobTitle, jobType,true); // (JobTitle)int.Parse(jobType);
            }
            switch (eJob)
            {
                case JobType.Admin:
                    src = "/Content/img/Male-Avatar-Bowler-Hat-icon.png";
                    break;
                case JobType.Runner:
                    src = "/Content/img/Male-Avatar-Cool-Cap-icon.png";
                    break;

            }
            //<img class="media-object" data-src="holder.js/64x64" alt="64x64" 
            //style="width: 32px; height: 32px;"
            //src = "~/Content/img/Male-Avatar-Cool-Cap-icon.png" >
            builder.MergeAttribute("src", src);
            builder.MergeAttribute("style", "width: 32px; height: 32px;");
            builder.MergeAttribute("class", "media-object");
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString ClientIdFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return MvcHtmlString.Create(
                htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression)));
        }

        public static MvcHtmlString TimeLineIcon(this HtmlHelper helper, int status)
        {
            var circleCss = "";
            var fa = "";
            var builder = new TagBuilder("div");
            var icon = new TagBuilder("i");

            //<div class="timeline-badge @timeline.Circle"><i class="fa @timeline.Icon fa-lg "></i></div>
            //                 <div class="timeline-panel">
            //                 @*   <div class="timeline-heading">@timeline.Title</div>*@
            //                    <div class="timeline-body">@timeline.Desc</div>                            
            //                 </div>
            //<img class="media-object" data-src="holder.js/64x64" alt="64x64" 
            //style="width: 32px; height: 32px;"
            //src = "~/Content/img/Male-Avatar-Cool-Cap-icon.png" >

            /*
              if (Status == Helper.TimeStatus.New)
                    return "default";
                if (Status == Helper.TimeStatus.ApporvallRequest || Status == Helper.TimeStatus.AcceptByRunner || Status == Helper.TimeStatus.AcceptByClient)
                    return "success";
                if (Status == Helper.TimeStatus.CancelByAdmin || Status == Helper.TimeStatus.Cancel || Status == Helper.TimeStatus.NoAcceptByClient || Status == Helper.TimeStatus.PrevStep)
                    return "danger";
                if (Status == Helper.TimeStatus.Confirm || Status == Helper.TimeStatus.Arrived || Status == Helper.TimeStatus.Close || Status == Helper.TimeStatus.ChangePrice)
                    return "info";
                return "default";
             */
            switch (status)
            {
                case Helper.TimeStatus.New:
                    fa = "fa-pencil";
                    circleCss = "default";
                    break;
                case Helper.TimeStatus.ApporvallRequest:
                    fa = "fa-paper-plane";
                    circleCss = "success";
                    break;
                case Helper.TimeStatus.Confirm:
                    fa = "fa-thumbs-o-up";
                    circleCss = "info";
                    break;
                case Helper.TimeStatus.CancelByAdmin:
                    fa = "fa-thumbs-o-down";
                    circleCss = "danger";
                    break;
                case Helper.TimeStatus.Cancel:
                    fa = "fa-times";
                    circleCss = "danger";
                    break;
                case Helper.TimeStatus.AcceptByRunner:
                    fa= "fa-road";
                    circleCss = "success";
                    break;
                case Helper.TimeStatus.Arrived:
                    fa= "fa-map-marker";
                    circleCss = "info";
                    break;
                case Helper.TimeStatus.NoAcceptByClient:
                    fa= "fa-hand-rock-o";
                    circleCss = "danger";
                    break;
                case Helper.TimeStatus.AcceptByClient:
                    fa= "fa-gift";
                    circleCss = "success";
                    break;
                default:
                    fa= "fa-rocket";
                    circleCss = "default";
                    break;

            }
            builder.AddCssClass("timeline-badge");
            builder.AddCssClass(circleCss);
            icon.AddCssClass("fa");
            icon.AddCssClass(fa);
            icon.AddCssClass("fa-lg");

            builder.InnerHtml = icon.ToString();





            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

    }


}