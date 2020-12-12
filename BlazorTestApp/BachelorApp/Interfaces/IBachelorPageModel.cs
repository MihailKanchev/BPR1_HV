using BachelorApp.Data;
using System;
using System.Threading.Tasks;

namespace BachelorApp.Interfaces
{
    public interface IBachelorPageModel
    {
        public Task<String> PutReadingItemAsync(Reading reading);
    }
}
