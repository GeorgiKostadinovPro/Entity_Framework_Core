namespace Boardgames.Data
{
    using Boardgames.Data.Models;
    using Microsoft.EntityFrameworkCore;
    
    public class BoardgamesContext : DbContext
    {
        public BoardgamesContext()
        { 
        }

        public BoardgamesContext(DbContextOptions<BoardgamesContext> options)
            : base(options) 
        {
        }

        public DbSet<Creator> Creators { get; set; }

        public DbSet<Seller> Sellers { get; set; }

        public DbSet<Boardgame> Boardgames { get; set; }

        public DbSet<BoardgameSeller> BoardgamesSellers { get; set; }

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
            modelBuilder.Entity<BoardgameSeller>(e =>
            {
                e.HasKey(bs => new { bs.BoardgameId, bs.SellerId });

                e.HasOne(bs => bs.Boardgame)
                .WithMany(bs => bs.BoardgamesSellers)
                .HasForeignKey(bs => bs.BoardgameId);

                e.HasOne(bs => bs.Seller)
                .WithMany(bs => bs.BoardgamesSellers)
                .HasForeignKey(bs => bs.SellerId);
            });
        }
    }
}
