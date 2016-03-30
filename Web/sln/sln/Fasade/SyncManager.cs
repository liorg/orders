using Michal.Project.Dal;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Fasade
{
    public class SyncManager
    {

        public void PushToClient(ApplicationDbContext context)
        {
        }

        public void PushToServer(ApplicationDbContext context)
        {
        }
    }

    //public class Factory<TResponse, TRequest> where TRequest : RequestItemSync<TRequest>
    //{
    //    public enum SYNType { Server, Client };
    //    private Factory() { }

    //    static readonly Dictionary<int, Func<TRequest, TResponse>> _dict
    //         = new Dictionary<int, Func<TRequest, TResponse>>();

    //    public static TResponse Create(int id, TRequest request)
    //    {
    //        Func<TRequest, TResponse> constructor = null;
    //        if (_dict.TryGetValue(id, out constructor))
    //            return constructor(request);

    //        throw new ArgumentException("No type registered for this id");
    //    }

    //    public static void Register(int id, Func<TRequest, TResponse> ctor)
    //    {
    //        _dict.Add(id, ctor);
    //    }
    //}
}