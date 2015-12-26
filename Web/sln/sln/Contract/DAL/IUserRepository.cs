using Michal.Project.DataModel;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract.DAL
{
    public interface IUserRepository
    {
        Task<UserLink> GetUserLink(Guid? userid);
        Task<UserDetail> GetUser(Guid? userid);
    }
}
