using Michal.Project.Contract.DAL;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Bll
{
    public class UserLogic
    {
        readonly IUserRepository _userRepository;
        public UserLogic(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<WhoAmI> UpadateQuick(WhoAmI user)
        {
            return await _userRepository.UpdateWhoAmI(user);
        }

        public async Task<WhoAmI> UpdateSync( ItemSync<WhoAmI> request)
        {
            return await _userRepository.UpdateWhoAmI(request.SyncObject);
        }
    }
}