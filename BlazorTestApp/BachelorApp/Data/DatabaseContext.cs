using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BachelorApp.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Console.WriteLine("Database connection succsessful.");
        }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Reading> Readings { get; set; }
        //Need to check if a factory should go here.
    }
    public class Sensor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int id { get; set; }
        [Required]
        public float temp { get; set; }
        [Required]
        public float pres { get; set; }
        [Required]
        public DateTime time { get; set; }
    }
    public class Reading
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [JsonIgnore]
        public int id { get; set; }
        [Required]
        public float P1StartQuantity { get; set; }
        [Required]
        public float P2StartQuantity { get; set; }
        [Required]
        public float P1OperatingTime { get; set; }
        [Required]
        public float P2OperatingTime { get; set; }
        [Required]
        public float Rain { get; set; }
        [Required]
        public float Niveau { get; set; }
        [Required]
        public int month { get; set; }
        [Required]
        public int day { get; set; }
        [Required]
        public int hour { get; set; }
        [Required]
        [JsonIgnore]
        public String label { get; set; }
        [Required]
        [JsonIgnore]
        public float probability { get; set; }
    }
}
