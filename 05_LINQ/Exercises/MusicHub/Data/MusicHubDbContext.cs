namespace MusicHub.Data;

using Microsoft.EntityFrameworkCore;
using MusicHub.Data.Models;

public class MusicHubDbContext : DbContext
{
    public MusicHubDbContext()
    {
    }

    public MusicHubDbContext(DbContextOptions<MusicHubDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Producer> Producers { get; set; }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Writer> Writers { get; set; }

    public virtual DbSet<Performer> Performers { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    public virtual DbSet<SongPerformer> SongsPerformers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlServer(Configuration.ConnectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Album>(e =>
        {
            e.HasOne(e => e.Producer)
            .WithMany(p => p.Albums)
            .HasForeignKey(e => e.ProducerId);
        });

        builder.Entity<SongPerformer>(e =>
        {
            e.HasKey(e => new { e.SongId, e.PerformerId });

            e.HasOne(e => e.Song)
            .WithMany(s => s.SongPerformers)
            .HasForeignKey(e => e.SongId);

            e.HasOne(e => e.Performer)
            .WithMany(s => s.PerformerSongs)
            .HasForeignKey(e => e.PerformerId);
        });

        builder.Entity<Song>(e =>
        {
            e.HasOne(e => e.Album)
            .WithMany(a => a.Songs)
            .HasForeignKey(e => e.AlbumId);

            e.HasOne(e => e.Writer)
            .WithMany(a => a.Songs)
            .HasForeignKey(e => e.WriterId);
        });
    }
}
