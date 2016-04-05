using Michal.Project.DataModel;
using Michal.Project.Models;
using Michal.Project.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract.DAL
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetCommentsShippingForRunner(Guid userId);
    
    }
}
