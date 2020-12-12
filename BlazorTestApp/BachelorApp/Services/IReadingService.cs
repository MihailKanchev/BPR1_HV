using BachelorApp.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BachelorApp.Services
{
    public interface IReadingService
    {
        public Task<List<Reading>> DisplayReadings();

        public Task AddReading(Reading reading);
    }
}
