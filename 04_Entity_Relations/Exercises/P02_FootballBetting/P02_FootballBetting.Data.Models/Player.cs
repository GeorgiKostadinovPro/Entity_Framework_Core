using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models;

public class Player
{
    public Player()
    {
        this.PlayersStatistics = new HashSet<PlayerStatistic>();
    }

    [Key]
    public int PlayerId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int SquadNumber { get; set; }

    [Required]
    public int Assists { get; set; }

    [Required]
    public bool IsInjured { get; set; }

    [Required]
    public int TownId { get; set; }

    public virtual Town Town { get; set; }

    [Required]
    public int PositionId { get; set; }

    public virtual Position Position { get; set; }

    [Required]
    public int TeamId { get; set; }

    public virtual Team Team { get; set; }

    public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; }
}
