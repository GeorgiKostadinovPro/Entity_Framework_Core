﻿namespace Artillery.Data
{
    using Artillery.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public class ArtilleryContext : DbContext
    {
        public ArtilleryContext() 
        { 
        }

        public ArtilleryContext(DbContextOptions<ArtilleryContext> options)
            : base(options) 
        { 
        }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<Shell> Shells { get; set; }

        public DbSet<Gun> Guns { get; set; }

        public DbSet<CountryGun> CountriesGuns { get; set; }

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString)
                    .UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CountryGun>(e =>
            {
                e.HasKey(e => new { e.CountryId, e.GunId });
            });

            modelBuilder.Entity<Manufacturer>(e =>
            {
                e.HasIndex(m => m.ManufacturerName)
                .IsUnique(true);
            });
        }
    }
}
