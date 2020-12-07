using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorApp
{
    public class Reading
    {
        public Reading(float p1s, float p1o, float p2s, float p2o, float rain, float niv, int m, int d, int h)
        {
            P1StartQuantity = p1s;
            P1OperatingTime = p1o;
            P2StartQuantity = p2s;
            P2OperatingTime = p2o;
            Rain = rain;
            Niveau = niv;
            month = m;
            day = d;
            hour = h;
        }
        public float P1StartQuantity { get; set; }
        public float P2StartQuantity { get; set; }
        public float P1OperatingTime { get; set; }
        public float P2OperatingTime { get; set; }
        public float Rain { get; set; }
        public float Niveau { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
    }
}
