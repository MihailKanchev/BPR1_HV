using BachelorApp.Data;
using BachelorApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BachelorApp.Interfaces
{
    public interface ILoraSocket
    {
        public ISensorDataService service { get; set; }
        public Task OpenSocket();
        public List<Sensor> GetList();
    }
}
