using Microsoft.EntityFrameworkCore;
using P02_FootballBetting.Data.Common;
using P02_FootballBetting.Data.Models;

namespace P02_FootballBetting.Data;

public class FootballBettingContext : DbContext
{
    public FootballBettingContext()
    {
    }

    public FootballBettingContext(DbContextOptions<FootballBettingContext> options)
        : base(options)
    {

    }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Town> Towns { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Player> Players { get; set; }
    
    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<PlayerStatistic> PlayersStatistics { get; set; }

    public virtual DbSet<Bet> Bets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(Config.ConnectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Town>(e =>
        {
            e.HasOne(e => e.Country)
            .WithMany(c => c.Towns)
            .HasForeignKey(e => e.CountryId);
        });

        modelBuilder.Entity<Bet>(e =>
        {
            e.HasOne(e => e.User)
            .WithMany(u => u.Bets)
            .HasForeignKey(e => e.UserId);

            e.HasOne(e => e.Game)
            .WithMany(g => g.Bets)
            .HasForeignKey(e => e.GameId);
        });

        modelBuilder.Entity<Player>(e =>
        {
            e.HasOne(e => e.Town)
            .WithMany(t => t.Players)
            .HasForeignKey(e => e.TownId);

            e.HasOne(e => e.Position)
            .WithMany(p => p.Players)
            .HasForeignKey(e => e.PositionId);

            e.HasOne(e => e.Team)
            .WithMany(t => t.Players)
            .HasForeignKey(e => e.TeamId);
        });

        modelBuilder.Entity<Team>(e =>
        {
            e.HasOne(e => e.Town)
            .WithMany(t => t.Teams)
            .HasForeignKey(e => e.TownId)
            .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(e => e.PrimaryKitColor)
            .WithMany(c => c.PrimaryKitTeams)
            .HasForeignKey(e => e.PrimaryKitColorId)
            .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(e => e.SecondaryKitColor)
            .WithMany(c => c.SecondaryKitTeams)
            .HasForeignKey(e => e.SecondaryKitColorId)
            .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Game>(e =>
        {
            e.HasOne(e => e.HomeTeam)
            .WithMany(ht => ht.HomeGames)
            .HasForeignKey(e => e.HomeTeamId)
            .OnDelete(DeleteBehavior.NoAction);

            e.HasOne(e => e.AwayTeam)
            .WithMany(ht => ht.AwayGames)
            .HasForeignKey(e => e.AwayTeamId)
            .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<PlayerStatistic>(e =>
        {
            e.HasKey(e => new { e.GameId, e.PlayerId });

            e.HasOne(e => e.Game)
            .WithMany(g => g.PlayersStatistics)
            .HasForeignKey(e => e.GameId);

            e.HasOne(e => e.Player)
            .WithMany(g => g.PlayersStatistics)
            .HasForeignKey(e => e.PlayerId);
        });
    }
}
