using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BachelorApp.Data
{
    public class ReadingService : IReadingService
    {
        protected readonly HVDBcontext _dbcontext;

        public ReadingService(HVDBcontext _db)
        {
            _dbcontext = _db;
        }

        public List<Reading> DisplayReadings()
        {
            return _dbcontext.reading.ToList();
        }

        public void AddReading(Reading reading)
        {
            _dbcontext.reading.Add(reading);
            _dbcontext.SaveChanges();
        }
    }
}
