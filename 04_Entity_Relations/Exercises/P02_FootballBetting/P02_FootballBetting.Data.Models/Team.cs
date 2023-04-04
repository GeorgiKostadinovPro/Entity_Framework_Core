using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models;

public class Team
{
    public Team()
    {
        this.Players = new HashSet<Player>();
        this.HomeGames = new HashSet<Game>();
        this.AwayGames = new HashSet<Game>();
    }

    [Key]
    public int TeamId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string LogoUrl { get; set; }

    [Required]
    public string Initials { get; set; }

    [Required]
    public decimal Budget { get; set; }

    [Required]
    public int PrimaryKitColorId { get; set; }

    public virtual Color PrimaryKitColor { get; set; }

    [Required]
    public int SecondaryKitColorId { get; set; }

    public virtual Color SecondaryKitColor { get; set; }

    [Required]
    public int TownId { get; set; }

    public virtual Town Town { get; set; }

    public virtual ICollection<Player> Players { get; set; }

    public virtual ICollection<Game> HomeGames { get; set; } 

    public virtual ICollection<Game> AwayGames { get; set; }
}
