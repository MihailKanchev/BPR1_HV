using BachelorApp.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BachelorApp.Interfaces
{
    public interface ILoraSocket
    {
        public Task OpenSocketAsync();
        public List<Sensor> GetList();
    }
}
