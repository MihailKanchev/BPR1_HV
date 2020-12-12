using System;
using System.Threading.Tasks;

namespace BachelorApp.Interfaces
{
    public interface ILoraSocket
    {
        public Task OpenSocketAsync();
        public void SaveMessage(String msg);
    }
}
