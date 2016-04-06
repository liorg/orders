using Michal.Project.Dal;
using Michal.Project.DataModel;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
namespace Michal.Project.Contract.DAL
{
    public interface ISupplierRepostory
    {
        List<Runner> GetRunners();
    }
}
