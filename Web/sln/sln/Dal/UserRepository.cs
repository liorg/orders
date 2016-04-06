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

        public Client FindClient(string clientId)
        {
            var client = _context.Client.Find(clientId);

            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken = _context.RefreshToken.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _context.RefreshToken.Add(token);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _context.RefreshToken.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _context.RefreshToken.Remove(refreshToken);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshToken.Remove(refreshToken);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _context.RefreshToken.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _context.RefreshToken.ToList();
        }

        public async Task<WhoAmI> GetMyDetail(Guid? userid)
        {
            if (userid == null || userid.Value == Guid.Empty)
                return await Task.FromResult<WhoAmI>(new WhoAmI { 
                                UserId = Guid.Empty.ToString(),
                                FullName = "Anonimous",
                                UserName = "Anonimous"
                });

            var result = await _context.Users.FirstOrDefaultAsync(u => u.Id == userid.Value.ToString());
            
            return new WhoAmI
            {
                UserId = result.Id,
                UserName = result.UserName,
                FullName=result.FirstName+" "+result.LastName
            };

        }

        public async Task<WhoAmI> UpdateWhoAmI(WhoAmI whoAmI)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Id == whoAmI.UserId.ToString());
            if (result != null)
            {
                result.FirstName = whoAmI.FullName;
                result.LastName = whoAmI.FullName;
                _context.Entry<ApplicationUser>(result).State = EntityState.Modified;
                
                return new WhoAmI
                {
                    UserId = result.Id,
                    UserName = result.UserName,
                    FullName = result.FirstName + " " + result.LastName
                };
            }
            return whoAmI;

        }
    
    }


}