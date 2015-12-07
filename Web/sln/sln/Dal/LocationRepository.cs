using Michal.Project.Contract.DAL;
using Michal.Project.DataModel;
using Michal.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using Michal.Project.Contract.Agent;
namespace Michal.Project.Dal
{
    public class LocationRepository : ILocationRepository
    {
        ApplicationDbContext _context; ILocationAgent _locationAgent;
        public LocationRepository(ApplicationDbContext context, ILocationAgent locationAgent)
        {
            _context = context;//ok
            _locationAgent = locationAgent;
        }

        public async Task<City> Get(string code)
        {
            var tryGet = await _context.City.Where(d => d.CityCode == code).FirstOrDefaultAsync();
            if (tryGet == null)
            {
                MemeryCacheDataService memory = new MemeryCacheDataService();
                var cityAgent = memory.GetCities().Where(c => c.Key == code).FirstOrDefault();
                City city = new City();
                city.IsActive = true;
                city.Name = cityAgent.Value;
                city.CityCode = cityAgent.Key;
                _context.City.Add(city);
                tryGet = city;
            }
            return tryGet;
        }

        public async Task<DistanceCities> GetDistance(string code1, string code2)
        {
            var tryGet = await _context.DistanceCities.Where(d => d.CityCode1 == code1 && d.CityCode2 == code2).FirstOrDefaultAsync();
            return tryGet;
        }

        public StreetsGeoLocation GetStreetsGeoLocation()
        {
            MemeryCacheDataService memory = new MemeryCacheDataService();
            return memory.GetStreetsGeoLocation();
        }


        void LazySet(City city)
        {
            //  throw new NotImplementedException();
        }

        void LazySetDistance(DistanceCities distanceCities)
        {
            //   throw new NotImplementedException();
        }
    }
}