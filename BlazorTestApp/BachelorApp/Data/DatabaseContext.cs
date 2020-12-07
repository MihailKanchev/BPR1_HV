using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BachelorApp.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Reading> Readings { get; set; }
        //Need to check if a factory should go here.
    }
    public class Sensor
    {
        public int sensorId { get; set; }
        public float temp { get; set; }
        public float pressure { get; set; }
        public DateTime time { get; set; }
    }
    public class Reading
    { 
        public int readingId { get; set; }
        public float p1StartTime { get; set; }
        public float p1OperatingTime { get; set; }
        public float p2StartTime { get; set; }
        public float p2OperatingTime { get; set; }
        public float rainInMM { get; set; }
        public float niveauInCM { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
        public float probability { get; set; }
        public string prediction { get; set; }
    }
}
