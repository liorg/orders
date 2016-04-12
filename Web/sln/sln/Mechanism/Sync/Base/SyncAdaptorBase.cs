using Michal.Project.Bll;
using Michal.Project.Contract.DAL;
using Michal.Project.Contract.View;
using Michal.Project.Dal;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Mechanism.Sync.Base
{
    public abstract class SyncAdaptorBase
    {
        protected ApplicationDbContext _context;

        public SyncAdaptorBase(ApplicationDbContext context)
        {
            _context = context;
        }

        protected SyncLogic GetLogic(ApplicationDbContext context)
        {
            IShippingRepository shippingRepository = new ShippingRepository(context);
            GeneralAgentRepository generalRepo = new GeneralAgentRepository(context);
            IUserRepository userRepository = new UserRepository(context);
            ICommentRepository commentRepository = new CommentRepository(context);
            ISyncRepository syncRepository = new SyncRepository(context);
            INotificationRepository notificationRepository = new NotificationRepository(context);

            return new SyncLogic(shippingRepository, userRepository, commentRepository, syncRepository, notificationRepository, generalRepo);
        }
    }

}