using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Practice5.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; } = default!;
        public DbSet<Sale> Sale { get; set; } = default!;
        public DbSet<Purchase> Purchase { get; set; } = default!;
        public DbSet<Inventory> Inventories { get; set; }

    }
}
