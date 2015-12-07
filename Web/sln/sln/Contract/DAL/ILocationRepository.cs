using Michal.Project.DataModel;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Michal.Project.Contract.DAL
{
    public interface ILocationRepository
    {
        //Task<City> Get(string code);
        
        //Task<DistanceCities> GetDistance(Address from, Address to);
        
        StreetsGeoLocation GetStreetsGeoLocation();

        Task SetLocationAsync(AddressEditorViewModel source, Address target);
        Task SetDistance(Address from, Address to,Shipping ship);
        bool IsChanged(AddressEditorViewModel addr);
        
    }
}
