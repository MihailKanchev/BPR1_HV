using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorApp.Data
{
    public class ReadingService
    {
        protected readonly HVDBcontext _dbcontext;

        public ReadingService(HVDBcontext _db)
        {
            _dbcontext = _db;
        }

        public List<Reading> displayReadings()
        {
            return _dbcontext.reading.ToList();
        }
    }
}
