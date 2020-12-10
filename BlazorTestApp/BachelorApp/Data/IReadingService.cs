using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorApp.Data
{
    public interface IReadingService
    {
        public List<Reading> DisplayReadings();

        public void AddReading(Reading reading);
    }
}
