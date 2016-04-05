using Michal.Project.Contract.DAL;
using Michal.Project.Dal;
using Michal.Project.DataModel;
using Michal.Project.Models;
using Michal.Project.Models.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using Michal.Project.Helper;
namespace Michal.Project.Dal
{
    public class CommentRepository : ICommentRepository
    {
        ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetCommentsShippingForRunner(Guid userId)
        {
            var comments =await (from c in _context.Comment
                           join s in _context.Shipping
                           on c.Shipping_ShippingId equals s.ShippingId
                           where s.GrantRunner == userId
                           select c).ToListAsync();
            return comments;
        }
    }
}

