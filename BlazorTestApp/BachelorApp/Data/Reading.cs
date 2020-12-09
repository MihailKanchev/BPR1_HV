using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
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
        public String label { get; set; }
        [Required]
        public float prediction { get; set; }
    }
}
