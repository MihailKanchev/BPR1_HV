using BachelorApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorApp.Services
{
    public class SensorDataService : ISensorDataService
    {
        private readonly IDbContextFactory<DatabaseContext> _contextFactory;

        public SensorDataService(IDbContextFactory<DatabaseContext> _db)
        {
            _contextFactory = _db;
        }

        public async Task<List<Sensor>> displayReadings()
        {
            List<Sensor> readings;
            using var context = _contextFactory.CreateDbContext();
            {
                readings = await context.Sensors.ToListAsync();
                //added an await and "Async" to ToList.  it was synchronous without the await.
            };

            return readings;
        }

        public async Task AddReading(Sensor reading)
        {
            using var context = _contextFactory.CreateDbContext();
            {
                context.Sensors.Add(reading);
                await context.SaveChangesAsync();
            };
        }

        public async Task AddReadingList(List<Sensor> sensors)
        {
            
            using var context = _contextFactory.CreateDbContext();
            {
                foreach (Sensor sensor in sensors)
                {
                    context.Sensors.Add(sensor);
                }
                await context.SaveChangesAsync();
            };
        }

    }
}
