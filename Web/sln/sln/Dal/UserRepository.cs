using Michal.Project.Contract;
using Michal.Project.Contract.DAL;
using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using Michal.Project.Models;
using Michal.Project.Helper;
namespace Michal.Project.Dal
{
    public class UserRepository : IUserRepository
    {
        ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserLink> GetUserLink(Guid? userid)
        {
            if (userid == null || userid.Value == Guid.Empty)
            {
                return await Task.FromResult<UserLink>(new UserLink { FullName = General.Empty, UserId = Guid.Empty }); 

            }
            
            var result= await _context.Users.FirstOrDefaultAsync(u => u.Id == userid.Value.ToString());
            return new UserLink{UserId=Guid.Parse(result.Id),FullName=result.FirstName+" "+result.LastName};

        }

        public async Task<UserDetail> GetUser(Guid? userid)
        {
            if (userid == null || userid.Value == Guid.Empty)
            {
                return await Task.FromResult<UserDetail>(new UserDetail {  UserId = Guid.Empty.ToString() });

            }
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Id == userid.Value.ToString());
            return new UserDetail
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                OrgId = result.Organization_OrgId.GetValueOrDefault(),
                Tel = result.Tel,
                UserId = result.Id,
                UserName = result.UserName,
                IsActive = result.IsActive,
                EmpId = String.IsNullOrEmpty(result.EmpId) ? General.Empty : result.EmpId
            };
        }
    }
}