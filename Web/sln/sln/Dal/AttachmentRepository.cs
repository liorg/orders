using Michal.Project.Contract;
using Michal.Project.Contract.DAL;
using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Entity;
namespace Michal.Project.Dal
{
    public class AttachmentRepository : IAttachmentRepository
    {
        ApplicationDbContext _context;
        public AttachmentRepository(ApplicationDbContext context)
        {
            _context = context;//ok
        }
        public async Task<AttachmentShipping> GetSign(Guid shipId)
        {
            var result=await _context.AttachmentShipping.Where(a => a.Shipping_ShippingId == shipId && a.IsSign == true && a.IsActive == true).FirstOrDefaultAsync();
            return result;
        }
        public AttachmentShipping UploadSign(Guid shipId, IUserContext user, string yourBase64String)
        {
            var dtNow = DateTime.Now;
            string basePath = System.Web.HttpContext.Current.ApplicationInstance.Server.MapPath("~/upload/") ;
            string path = Path.Combine(basePath, dtNow.ToString("yyyyMMdd"), shipId.ToString());
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            var id=Guid.NewGuid();
            var filename = id.ToString()+".PNG";
            path = path+"//"+ filename;
            File.WriteAllBytes(path, Convert.FromBase64String(yourBase64String));
            //System.Web.HttpRuntime.AppDomainAppVirtualPath +
            var baseUrl =  "/upload" + "/" + dtNow.ToString("yyyyMMdd") + "/" + shipId.ToString() + "/" + filename;

           // var url = new Uri(baseUrl);

            AttachmentShipping addAttachment = new AttachmentShipping();
            addAttachment.CommentId = id;
            addAttachment.CreatedBy = user.UserId;
            addAttachment.ModifiedBy = user.UserId;

            addAttachment.CreatedOn = dtNow;
            addAttachment.ModifiedOn = dtNow;
            addAttachment.Name = "חתימה";
            addAttachment.Path = baseUrl;//url.OriginalString;
            addAttachment.TypeMime = "image/png";
            addAttachment.IsActive = true;
            addAttachment.Shipping_ShippingId = shipId;
            addAttachment.IsSign = true;
           
            return addAttachment;
        }
    }
}