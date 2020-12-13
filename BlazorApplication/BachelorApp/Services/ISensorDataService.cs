using BachelorApp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BachelorApp.Services
{
    public interface ISensorDataService
    {
        public Task<List<Sensor>> displayReadings();

        public Task AddReading(Sensor data);
    }
}
