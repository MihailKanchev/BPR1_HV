using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorApp.Data
{
    public interface ISensorDataService
    {
        public List<Sensor> displayReadings();

        public void AddReading(Sensor data);
    }
}
