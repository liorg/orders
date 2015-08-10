using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sln.Helper
{
    public static class ImageHelper
    {
        public static MvcHtmlString Image(this HtmlHelper helper, string jobType)
        {
            var builder = new TagBuilder("img");
            var src = "/Content/img/Male-Avatar-Emo-Haircut-icon.png";
            JobType eJob = JobType.Client;
            if (!String.IsNullOrWhiteSpace(jobType))
            {
                int ijob=int.Parse(jobType);
                eJob= (JobType)ijob;
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
           // builder.MergeAttribute("alt", altText);
            //builder.MergeAttribute("height", height);
           // builder.MergeAttribute("height", height);
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}