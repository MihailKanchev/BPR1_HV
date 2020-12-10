using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BachelorApp.Data
{
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
