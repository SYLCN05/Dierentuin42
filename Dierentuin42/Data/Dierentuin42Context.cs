using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dierentuin42.Models;

namespace Dierentuin42.Data
{
    public class Dierentuin42Context : DbContext
    {
        public Dierentuin42Context (DbContextOptions<Dierentuin42Context> options)
            : base(options)
        {
        }

        public DbSet<Dierentuin42.Models.Category> Category { get; set; } = default!;
        public DbSet<Dierentuin42.Models.Zoo> Zoo { get; set; } = default!;
        public DbSet<Dierentuin42.Models.Animal> Animal { get; set; } = default!;
        public DbSet<Dierentuin42.Models.Enclosure> Enclosure { get; set; } = default!;
    }
}
