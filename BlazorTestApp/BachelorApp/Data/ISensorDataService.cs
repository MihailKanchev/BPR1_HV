using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorApp.Data
{
    public interface ISensorDataService
    {
        public List<SensorData> displayReadings();

        public void AddReading(SensorData data);
    }
}
