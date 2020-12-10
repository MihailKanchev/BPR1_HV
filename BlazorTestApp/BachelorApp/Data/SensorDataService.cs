using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorApp.Data
{
    public class SensorDataService : ISensorDataService
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

        public void AddReading(SensorData data)
        {
            _dbcontext.sensor.Add(data);
            _dbcontext.SaveChanges();
        }
    }
}
