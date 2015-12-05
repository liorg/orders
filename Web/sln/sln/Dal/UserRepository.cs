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
                return await Task.FromResult<UserLink>(new UserLink { FullName = "", UserId = Guid.Empty }); 

            }
            
            var result= await _context.Users.FirstOrDefaultAsync(u => u.Id == userid.Value.ToString());
            return new UserLink{UserId=Guid.Parse(result.Id),FullName=result.FirstName+" "+result.LastName};

        }
    }
}