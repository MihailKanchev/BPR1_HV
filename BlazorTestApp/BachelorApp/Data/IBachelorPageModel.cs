using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorApp.Data
{
    public interface IBachelorPageModel
    {
        public Task CollectSensorDataAsync();

        public Task<String> PutReadingItemAsync(Reading reading);
    }
}
