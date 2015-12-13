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
                    fa = "fa-road";
                    circleCss = "success";
                    break;
                case Helper.TimeStatus.Arrived:
                    fa = "fa-clock-o";// "fa-map-marker";
                    circleCss = "info";
                    break;
                case Helper.TimeStatus.ArrivedSender:
                    fa = "fa-clock-o";//"fa-check";
                    circleCss = "info";
                    break;
                //fa-check
                case Helper.TimeStatus.NoAcceptByClient:
                    fa = "fa-hand-rock-o";
                    circleCss = "danger";
                    break;
                case Helper.TimeStatus.AcceptByClient:
                    fa = "fa-gift";
                    circleCss = "success";
                    break;
                default:
                    fa = "fa-rocket";
                    circleCss = "success";
                    break;

            }
            builder.AddCssClass("timeline-badge");
            builder.AddCssClass(circleCss);
            icon.AddCssClass("fa");
            icon.AddCssClass(fa);
            icon.AddCssClass("fa-lg");
            icon.ToString(TagRenderMode.EndTag);
            builder.InnerHtml += icon.ToString();
            return MvcHtmlString.Create(builder.ToString());
            //return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString Cyrcle(this HtmlHelper helper, double value, string size)
        {
            var builder = new TagBuilder("div");
            var span = new TagBuilder("span");
            var slice = new TagBuilder("div");
            var bar = new TagBuilder("div");
            var fill = new TagBuilder("div");

            string cssValue = "p" + ((int)value).ToString("");

            if (!String.IsNullOrWhiteSpace(size))
                builder.AddCssClass(size);
            builder.AddCssClass("c100");
            builder.AddCssClass(cssValue);

            span.SetInnerText(((int)value).ToString() + "%");
            span.ToString(TagRenderMode.EndTag);
            builder.InnerHtml += span.ToString();

            slice.AddCssClass("slice");

            bar.AddCssClass("bar");
            bar.ToString(TagRenderMode.EndTag);
            slice.InnerHtml += bar.ToString();

            fill.AddCssClass("fill");
            fill.ToString(TagRenderMode.EndTag);
            slice.InnerHtml += fill.ToString();

            builder.InnerHtml += slice.ToString();

            return MvcHtmlString.Create(builder.ToString());

            /*
            <div class=" c100 p77 big">
                <span>77</span>
                <div class="slice">
                    <div class="bar"></div>
                    <div class="fill"></div>
                </div>
            </div>
     
             */
        }

    }


}