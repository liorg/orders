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
using Kipodeal.Helper.Cache;
namespace Michal.Project.Dal
{
    public class LocationRepository : ILocationRepository
    {
        const string cacheLocation = "~/cachelocation.txt";
        ApplicationDbContext _context; ILocationAgent _locationAgent;
        public LocationRepository(ApplicationDbContext context, ILocationAgent locationAgent)
        {
            _context = context;//ok
            _locationAgent = locationAgent;
        }

        public bool IsChanged(AddressEditorViewModel addr)
        {
            if (addr.Citycode != addr.CitycodeOld || addr.Streetcode != addr.StreetcodeOld || addr.Num != addr.NumOld)
                return true;
            return false;

        }

        public async Task SetDistance(Address from, Address to, Shipping ship)
        {
            DistanceCities distanceCities = await GetDistance(from, to);
            if (distanceCities != null)
            {
                ship.DistanceText = distanceCities.DistanceText;
                ship.DistanceValue = distanceCities.DistanceValue;
                ship.FixedDistanceValue = distanceCities.FixedDistanceValue;
            }
        }
        
        async Task<DistanceCities> GetDistance(Address from, Address to)
        {
            ////trace city from
            //var cityFrom = await Get(from.CityCode);
            ////trace city to
            //var cityTo = await Get(to.CityCode);

            var distances = await GetDistancesDb();
            var tryGet = distances.Where(d => (d.CityCode1 == from.CityCode && d.CityCode2 == to.CityCode) || (d.CityCode2 == from.CityCode && d.CityCode1 == to.CityCode)).FirstOrDefault();
            if (tryGet == null)
            {
                tryGet = await _locationAgent.FindDistance(from, to);
                await SetDistanceCitiesDb(tryGet);
            }
            return tryGet;
        }

        public StreetsGeoLocation GetStreetsGeoLocation()
        {
            MemeryCacheDataService memory = new MemeryCacheDataService();
            return memory.GetStreetsGeoLocation();
        }

        void LazySetDistance(DistanceCities distanceCities)
        {
            //   throw new NotImplementedException();
        }

        async Task<List<City>> GetCitiesDb()
        {
            CacheMemoryProvider cacheMemoryProvider = new CacheMemoryProvider();
            List<City> lists = null;
            cacheMemoryProvider.Get("GetCitiesDb", out lists);
            if (lists == null)
            {
                lists = await _context.City.Where(s => s.IsActive == true).ToListAsync();
                cacheMemoryProvider.Set("GetCitiesDb", lists, null, cacheLocation);
            }
            return lists;
        }

        async Task<City> Get(string code)
        {
            var cities = await GetCitiesDb();
            var tryGet = cities.Where(d => d.CityCode == code).FirstOrDefault();
            if (tryGet == null)
            {
                MemeryCacheDataService memory = new MemeryCacheDataService();
                var cityAgent = memory.GetCities().Where(c => c.Key == code).FirstOrDefault();
                City city = new City();
                city.IsActive = true;
                city.Name = cityAgent.Value;
                city.CityCode = cityAgent.Key;
                await SetCitiesDb(city);
                tryGet = city;
            }
            return tryGet;
        }


        async Task SetCitiesDb(City city)
        {
            CacheMemoryProvider cacheMemoryProvider = new CacheMemoryProvider();

            _context.City.Add(city);
            await _context.SaveChangesAsync();
            cacheMemoryProvider.Refresh(cacheLocation);
            //  await _context.SaveChangesAsync();
        }

        async Task SetDistanceCitiesDb(DistanceCities distanceCities)
        {
            if (distanceCities == null)
                return;
            CacheMemoryProvider cacheMemoryProvider = new CacheMemoryProvider();
            var tryGet = await _context.DistanceCities.Where(d => d.IsActive == true && d.CityCode1 == distanceCities.CityCode1 && d.CityCode2 == distanceCities.CityCode2).FirstOrDefaultAsync();
            if (tryGet == null)
            {
                _context.DistanceCities.Add(distanceCities);
                await _context.SaveChangesAsync();
            }
            
            cacheMemoryProvider.Refresh(cacheLocation);

        }

        async Task<List<DistanceCities>> GetDistancesDb()
        {
            CacheMemoryProvider cacheMemoryProvider = new CacheMemoryProvider();
            List<DistanceCities> lists = null;
            cacheMemoryProvider.Get("GetDistancesDb", out lists);
            if (lists == null)
            {
                lists = await _context.DistanceCities.Where(s => s.IsActive == true).ToListAsync();
                cacheMemoryProvider.Set("GetDistancesDb", lists, null, cacheLocation);
            }
            return lists;
        }

        public async Task SetLocationAsync(AddressEditorViewModel source, Address target)
        {
            await _locationAgent.SetLocationAsync(this, source, target);
        }
    }
}