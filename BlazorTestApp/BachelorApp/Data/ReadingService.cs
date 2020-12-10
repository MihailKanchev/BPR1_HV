using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BachelorApp.Data
{
    public class ReadingService : IReadingService
    {
        protected readonly DatabaseContext _dbcontext;

        public ReadingService(DatabaseContext _db)
        {
            _dbcontext = _db;
        }

        public List<Reading> DisplayReadings()
        {
            return _dbcontext.Readings.ToList();
        }

        public void AddReading(Reading reading)
        {
            _dbcontext.Readings.Add(reading);
            _dbcontext.SaveChanges();
        }
    }
}
