using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models;

public class Album
{
    public Album()
    {
        this.Songs = new HashSet<Song>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(40)]
    public string Name { get; set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    public decimal Price => this.Songs.Any()
        ? this.Songs.Sum(s => s.Price)
        : 0.0m;

    public int? ProducerId { get; set; }

    public virtual Producer Producer { get; set; }
    
    public virtual ICollection<Song> Songs { get; set; }
}
