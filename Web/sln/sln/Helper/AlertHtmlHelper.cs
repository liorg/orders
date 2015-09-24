using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
namespace Michal.Project.Helper
{
    ///https://www.simple-talk.com/dotnet/asp.net/writing-custom-html-helpers-for-asp.net-mvc/
    ///https://github.com/EdCharbeneau/FluentHtmlHelpers/blob/SimpleTalkArticle/FluentHtmlHelpers/MyHelpers/AlertHtmlHelper.cs
    /// <summary>
    /// Generates an Alert message
    /// </summary>
    public static class AlertHtmlHelper
    {
        /// <summary>
        /// Generates an Alert message
        /// </summary>
        public static AlertBox Alert(this HtmlHelper html,
            string text,
            AlertStyle alertStyle = AlertStyle.Default,
            bool hideCloseButton = false,
            object htmlAttributes = null
            )
        {
            return new AlertBox(text, alertStyle, hideCloseButton, htmlAttributes);
        }

        // Strongly typed
        /// <summary>
        /// Generates an Alert message
        /// </summary>
        public static AlertBox AlertFor<TModel, TTextProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TTextProperty>> expression,
            AlertStyle alertStyle = AlertStyle.Default,
            bool hideCloseButton = false,
            object htmlAttributes = null
            )
        {
        
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            return new AlertBox((string)metadata.Model, alertStyle, hideCloseButton, htmlAttributes);
        }

        /// <summary>
        /// Generates an Alert message
        /// </summary>
        public static AlertBox AlertFor<TModel, TTextProperty, TStyleProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TTextProperty>> textExpression,
            Expression<Func<TModel, TStyleProperty>> styleExpression,
            bool hideCloseButton = false,
            object htmlAttributes = null
            )
        {
            var text = (string)ModelMetadata.FromLambdaExpression(textExpression, html.ViewData).Model;
            var alertStyle = (AlertStyle)ModelMetadata.FromLambdaExpression(styleExpression, html.ViewData).Model;

            return new AlertBox(text, alertStyle, hideCloseButton, htmlAttributes);
        }


    }

   

    public interface IAlertBoxFluentOptions : IHtmlString
    {
        IAlertBoxFluentOptions HideCloseButton(bool hideCloseButton = true);
        IAlertBoxFluentOptions Attributes(object htmlAttributes);
    }

    public interface IAlertBox : IHtmlString, IAlertBoxFluentOptions
    {
        IAlertBoxFluentOptions Success();
        IAlertBoxFluentOptions Warning();
        IAlertBoxFluentOptions Info();
    }

    public class AlertBoxFluentOptions : IHtmlString, IAlertBoxFluentOptions
    {
        private readonly AlertBox parent;

        public AlertBoxFluentOptions(AlertBox parent)
        {
            this.parent = parent;
        }

        public IAlertBoxFluentOptions HideCloseButton(bool hideCloseButton = true)
        {
            return parent.HideCloseButton(hideCloseButton);
        }

        public IAlertBoxFluentOptions Attributes(object htmlAttributes)
        {
            return parent.Attributes(htmlAttributes);
        }

        public override string ToString()
        {
            return parent.ToString();
        }

        public string ToHtmlString()
        {
            return ToString();
        }
    }

    public class AlertBox : IAlertBox
    {
        private readonly string text;

        private AlertStyle alertStyle;

        private bool hideCloseButton;

        private object htmlAttributes;

        /// <summary>
        /// Returns a div alert box element with the options specified
        /// </summary>
        /// <param name="text">Sets the text to display</param>
        /// <param name="style">Sets style of alert box [Default | Success | Warning | Info ]</param>
        /// <param name="hideCloseButton">Sets the close button visibility</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        public AlertBox(string text, AlertStyle style, bool hideCloseButton = false, object htmlAttributes = null)
        {
            this.text = text;
            this.alertStyle = style;
            this.hideCloseButton = hideCloseButton;
            this.htmlAttributes = htmlAttributes;
        }

        #region FluentAPI
        /// <summary>
        /// Sets the display style to Success
        /// </summary>
        public IAlertBoxFluentOptions Error()
        {
            alertStyle = AlertStyle.Error;
            return new AlertBoxFluentOptions(this);
        }
        /// <summary>
        /// Sets the display style to Success
        /// </summary>
        public IAlertBoxFluentOptions Success()
        {
            alertStyle = AlertStyle.Success;
            return new AlertBoxFluentOptions(this);
        }

        /// <summary>
        /// Sets the display style to Warning
        /// </summary>
        /// <returns></returns>
        public IAlertBoxFluentOptions Warning()
        {
            alertStyle = AlertStyle.Warning;
            return new AlertBoxFluentOptions(this);
        }

        /// <summary>
        /// Sets the display style to Info
        /// </summary>
        /// <returns></returns>
        public IAlertBoxFluentOptions Info()
        {
            alertStyle = AlertStyle.Info;
            return new AlertBoxFluentOptions(this);
        }

        /// <summary>
        /// Sets the close button visibility
        /// </summary>
        /// <returns></returns>
        public IAlertBoxFluentOptions HideCloseButton(bool hideCloseButton = true)
        {
            this.hideCloseButton = hideCloseButton;
            return new AlertBoxFluentOptions(this);
        }

        /// <summary>
        /// An object that contains the HTML attributes to set for the element.
        /// </summary>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public IAlertBoxFluentOptions Attributes(object htmlAttributes)
        {
            this.htmlAttributes = htmlAttributes;
            return new AlertBoxFluentOptions(this);
        }
        #endregion //FluentAPI

        private string RenderAlert()
        {
            //<div class="alert-box">
            var wrapper = new TagBuilder("div");
            //merge attributes
            wrapper.MergeAttributes(htmlAttributes != null ? HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes) : null);

            //if (alertStyle != AlertStyle.Default)
            //    wrapper.AddCssClass(alertStyle.ToString().ToLower());
            switch (alertStyle)
            {
                case AlertStyle.Default:
                    wrapper.AddCssClass("alert-info");
                    break;
                case AlertStyle.Success:
                    wrapper.AddCssClass("alert-success");
                    break;
                case AlertStyle.Warning:
                    wrapper.AddCssClass("alert-warning");
                    break;
                case AlertStyle.Error:
                    wrapper.AddCssClass("alert-danger");
                    break;
                case AlertStyle.Info:
                    wrapper.AddCssClass("alert-info");
                    break;
                default:
                    wrapper.AddCssClass("alert-info");
                    break;
            }
            wrapper.AddCssClass("alert");


            //build html
            wrapper.InnerHtml = text;

            //Add close button
            if (!hideCloseButton)
                wrapper.InnerHtml += RenderCloseButton();

            return wrapper.ToString();
        }

        private static TagBuilder RenderCloseButton()
        {
            //   <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            //<a href="" class="close">x</a>
            var closeButton = new TagBuilder("a");
            closeButton.AddCssClass("close");
            closeButton.Attributes.Add("href", "");
            closeButton.Attributes.Add("data-dismiss", "alert");
            closeButton.Attributes.Add("aria-label", "סגור");
            closeButton.InnerHtml = "&times;";
            return closeButton;
        }

        //Render HTML
        public override string ToString()
        {
            return RenderAlert();
        }

        //Return ToString
        public string ToHtmlString()
        {
            return ToString();
        }
    }
}