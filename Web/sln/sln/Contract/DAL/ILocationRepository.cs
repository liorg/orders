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
        Task<City> Get(string code);
        //void LazySet(City city);

        Task<DistanceCities> GetDistance(string code1 ,string  code2);
        //void LazySetDistance(DistanceCities distanceCities);

        StreetsGeoLocation GetStreetsGeoLocation();
        
    }
}
