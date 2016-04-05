using Michal.Project.Bll;
using Michal.Project.Contract.DAL;
using Michal.Project.Contract.View;
using Michal.Project.Dal;
using Michal.Project.Mechanism.Sync.Base;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace Michal.Project.Mechanism.Sync.Base
{
    public abstract class PushAdaptor : SyncAdaptorBase
    {
        Dictionary<Guid, int> _users;

        protected Dictionary<Guid, int> Users
        {
            get
            {
                return _users;
            }
        }
        SyncLogic _syncLogic;
  
        public PushAdaptor(ApplicationDbContext context)
            : base(context)
        {
            _syncLogic = GetLogic(context);
            _users = new Dictionary<Guid, int>();
            NotifyUsers(_users);

        }

        protected abstract void NotifyUsers(Dictionary<Guid, int> users);

        public virtual async Task Push(ISyncItem request)
        {
            foreach (var user in _users)
            {

            }
        }

        public abstract Task SyncAll(ISync request);

        protected SyncLogic GetLogic(ApplicationDbContext context)
        {
            IShippingRepository shippingRepository = new ShippingRepository(context);
            GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
            IUserRepository userRepository = new UserRepository(context);
            ICommentRepository commentRepository = new CommentRepository(context);
            ISyncRepository syncRepository = new SyncRepository(context);

            return new SyncLogic(shippingRepository, userRepository, commentRepository, syncRepository);
        }
    }

}