using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract.DAL
{
    public interface IAttachmentRepository
    {
        Task<AttachmentShipping> GetSign(Guid shipId);
        AttachmentShipping UploadSign(Guid shipId, IUserContext user, string yourBase64String);
    }
}
