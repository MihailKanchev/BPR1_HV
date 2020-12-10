using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorApp.Data
{
    public class SensorDataService : ISensorDataService
    {
        protected readonly DatabaseContext _dbcontext;

        public SensorDataService(DatabaseContext _db)
        {
            _dbcontext = _db;
        }

        public List<Sensor> displayReadings()
        {
            return _dbcontext.Sensors.ToList();
        }

        public void AddReading(Sensor data)
        {
            _dbcontext.Sensors.Add(data);
            _dbcontext.SaveChanges();
        }
    }
}
