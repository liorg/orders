﻿using Michal.Project.Contract;
using Michal.Project.Contract.DAL;
using Michal.Project.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
namespace Michal.Project.Dal
{
    public class OfferRepository : IOfferRepository
    {
        ApplicationDbContext _context;
        public OfferRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(Guid modifiedId, RequestShipping request, List<RequestItemShip> requestItemShips)
        {
            _context.RequestShipping.Add(request);
            foreach (var requestItem in requestItemShips)
            {
                _context.RequestItemShip.Add(requestItem);
            }
            // await  _context.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(Guid modifiedId, RequestShipping request, List<RequestItemShip> requestItemShips, bool deleteChildrens)
        {
            var model = await GetOfferAndHisChilds(request.RequestShippingId); // _context.RequestShipping.Include(s => s.RequestItemShip).FirstOrDefaultAsync(f => f.RequestShippingId == request.RequestShippingId);
            model.StatusCode = request.StatusCode;
            if (deleteChildrens)
            {
                foreach (var requestItemToDel in model.RequestItemShip)
                {
                    model.RequestItemShip.Remove(requestItemToDel);
                    _context.Entry<RequestItemShip>(requestItemToDel).State = EntityState.Deleted;
                }
                _context.Entry<RequestShipping>(model).State = EntityState.Modified;

                foreach (var requestItem in requestItemShips)
                    _context.RequestItemShip.Add(requestItem);

            }

            //  await _context.SaveChangesAsync();
        }


        public async Task<RequestShipping> GetOfferAndHisChilds(Guid requestShippingId)
        {
            return await _context.RequestShipping.Include(s => s.RequestItemShip).FirstOrDefaultAsync(f => f.RequestShippingId == requestShippingId);
        }

        public async Task<RequestShipping> GetOffer(Guid requestShippingId)
        {
            return await _context.RequestShipping.FirstOrDefaultAsync(f => f.RequestShippingId == requestShippingId);
        }
    }
}