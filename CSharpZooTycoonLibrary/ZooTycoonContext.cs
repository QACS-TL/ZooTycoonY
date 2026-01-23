using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpZooTycoonLibrary
{
    public class ZooTycoonContext: DbContext
    {
        public DbSet<Animal> Animals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>()
                .HasDiscriminator<string>("AnimalType")
                .HasValue<Dog>("Dog")
                .HasValue<Cat>("Cat")
                .HasValue<Bird>("Bird");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(local);Database=ZooTycoon;Trusted_Connection=True;Encrypt=False");
        }
    }
}
