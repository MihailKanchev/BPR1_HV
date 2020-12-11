using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorApp.Data
{
    public interface IReadingService
    {
        public Task<List<Reading>> DisplayReadings();

        public Task AddReading(Reading reading);
    }
}
