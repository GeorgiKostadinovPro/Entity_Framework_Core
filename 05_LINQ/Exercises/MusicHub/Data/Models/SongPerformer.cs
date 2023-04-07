using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models;

public class SongPerformer
{
    [Required]
    public int SongId { get; set; }

    public virtual Song Song { get; set; }

    [Required]
    public int PerformerId { get; set; }    

    public virtual Performer Performer { get; set; }
}
