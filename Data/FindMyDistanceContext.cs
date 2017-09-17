using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FindMyDistance.Models
{
    public class FindMyDistanceContext : DbContext
    {
        public FindMyDistanceContext (DbContextOptions<FindMyDistanceContext> options)
            : base(options)
        {
        }

        public DbSet<FindMyDistance.Models.Distance> Distance { get; set; }
    }
}
