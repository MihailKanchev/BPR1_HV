using BachelorApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace BachelorApp.Services
{
    public class ReadingService : IReadingService
    {
        private readonly IDbContextFactory<DatabaseContext> _contextFactory;

        public ReadingService(IDbContextFactory<DatabaseContext> _db)
        {
            _contextFactory = _db;
        }

        public async Task<List<Reading>> DisplayReadings()
        {
            List<Reading> readings;
            using var context = _contextFactory.CreateDbContext();
            {
                readings = context.Readings.ToList();
                //added an await and "Async" to ToList.  it was synchronous without the await.
            };
            
            return readings;
        }

        public async Task AddReading(Reading reading)
        {
            using var context = _contextFactory.CreateDbContext();
            {
                context.Readings.Add(reading);
                await context.SaveChangesAsync();
            };
        }
    }
}
