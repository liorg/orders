using Michal.Project.Contract.DAL;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract.Agent
{
    public interface ILocationAgent
    {
        Task SetLocationAsync(ILocationRepository locationRepository,AddressEditorViewModel source, Michal.Project.DataModel.Address target);

        Task FindDistance(Michal.Project.DataModel.Address from, Michal.Project.DataModel.Address to);
    }
}
