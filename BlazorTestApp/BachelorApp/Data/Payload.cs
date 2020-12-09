using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorApp.Data
{
    public class Payload
    {
        public String cmd { get; set; }
        public String EUI { get; set; }
        public long port { get; set; }
        public String data { get; set; }
    }
}
