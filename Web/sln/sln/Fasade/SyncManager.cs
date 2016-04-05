using Michal.Project.Contract.View;
using Michal.Project.Dal;
using Michal.Project.Mechanism.Sync.Base;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Michal.Project.Fasade
{
    public class SyncManager
    {
        public async Task Push(ISyncItem request,PushAdaptor pushAdaptor)
        {
            await pushAdaptor.Push(request);
        }

        public async Task<IEnumerable<T>>  pull<T>(ISync request,PollAdaptor<T> pullAdaptor) where T : ISyncItem
        {
            return await pullAdaptor.Poll(request);
        }

        public async Task<T> pull<T>(ISyncItem request, PollAdaptor<T> pullAdaptor) where T : ISyncItem
        {
            return await pullAdaptor.PollItem(request);
        }

        public async Task Sync(ISync request, PushAdaptor syncAllAdaptor)
        {
            await syncAllAdaptor.SyncAll(request);
        }
         
    }
}