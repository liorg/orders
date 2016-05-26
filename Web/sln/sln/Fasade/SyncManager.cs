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
        public UserContext CurrentUser { get;protected set; }

        public SyncManager()
        {

        }
        public SyncManager(UserContext userContext)
        {
            CurrentUser = userContext;
        }
        /// <summary>
        /// Notify To users over GSM and save records On Db
        /// </summary>
        /// <param name="pushAdaptor"></param>
        /// <returns></returns>
        public async Task Push(PushAdaptor pushAdaptor)
        {
            try
            {
                await pushAdaptor.Push(CurrentUser);
            }
            catch (Exception e)
            {

                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
            }
         
        }

        /// <summary>
        /// Get Only Records that's changed by userid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="pullAdaptor"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>>  pull<T>(ISync request,PollAdaptor<T> pullAdaptor) where T : ISyncItem
        {
            return await pullAdaptor.Poll(request);
        }

        /// <summary>
        ///  Get Only Record that changed by current
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="pullAdaptor"></param>
        /// <returns></returns>
        public async Task<T> pull<T>(ISyncItem request, PollAdaptor<T> pullAdaptor) where T : ISyncItem
        {
            return await pullAdaptor.PollItem(request);
        }

        /// <summary>
        /// send to system that's 
        /// </summary>
        /// <param name="syncAllAdaptor"></param>
        /// <returns></returns>
        public async Task Sync(PushAdaptor syncAllAdaptor)
        {
            await syncAllAdaptor.SyncAll(CurrentUser);
        }

        /// <summary>
        /// Register new user and also add records that's belong to him
        /// </summary>
        /// <param name="request"></param>
        /// <param name="registerAdaptor"></param>
        /// <returns></returns>
        public async Task Register(ISync request, RegisterAdaptor registerAdaptor)
        {
            try
            {
                await registerAdaptor.NotifyDataToUser(request);
            }
            catch (Exception e)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
            }
        }


        
    }
}