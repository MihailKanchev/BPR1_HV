using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorApp.Data
{
    public class HVDBcontext : DbContext
    {
        public HVDBcontext(DbContextOptions<HVDBcontext> options) : base(options)
        {
            Console.WriteLine("Database connection succsessful.");
        }

        public DbSet<Reading> reading { get; set; }
    }
}
