using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class Town
{
    public Town()
    {
        this.Players = new HashSet<Player>();
        this.Teams = new HashSet<Team>();
    }

    [Key]
    public int TownId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int CountryId { get; set; }

    public virtual Country Country { get; set; }

    public virtual ICollection<Player> Players { get; set; }

    public virtual ICollection<Team> Teams { get; set; }
}
