using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorApp.Data
{
    public class SensorDataService
    {
        protected readonly HVDBcontext _dbcontext;

        public SensorDataService(HVDBcontext _db)
        {
            _dbcontext = _db;
        }

        public List<SensorData> displayReadings()
        {
            return _dbcontext.sensor.ToList();
        }
    }
}
