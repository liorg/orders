using Michal.Project.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
    public class Notify
    {
        public List<NotifyItem> Items { get; set; }
        public string Url { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }

        public bool ShowParent
        {
            get
            {
                if ((Items != null && Items.Any() && Items.Count > 1) || (Items == null) || (Items != null && Items.Count == 0))
                {
                    return true;
                }
                return false;
            }
        }

    }

    public class NotifyItem
    {
        public const string MESSAGE_DEFAULT = "DEFAULT";
        public const string MESSAGE_COMMENT = "COMMENT";
        public const string MESSAGE_ORDER = "ORDER";
        public const string MESSAGE_CHANGEUSER = "CHANGEUSER";
        public Guid Id { get; set; }
        [Display(Name = "כותרת")]
        public string Title { get; set; }
        [Display(Name = "תוכן")]
        public string Body { get; set; }
        [Display(Name = "לינק")]
        public string Url { get; set; }
        [Display(Name = "הודעה נצפתה")]
        public bool IsRead { get; set; }

        public DateTime CreatedOn { get; set; }
        [Display(Name = "ת. יצירה")]
        public string CreatedOnTxt
        {
            get
            {
                if (CreatedOn != DateTime.MinValue)
                {
                    if (CreatedOn.Date == DateTime.Now.Date)
                        return CreatedOn.ToString("HH:mm");
                    return CreatedOn.ToString("dd-MM-yy HH:mm");
                }

                return General.Empty;
            }
        }

        [Display(Name = "סוג הודעה")]
        public string TypeMessage { get; set; }

        [Display(Name = "מזהה רשומה")]
        public string RecID { get; set; }
    }
}