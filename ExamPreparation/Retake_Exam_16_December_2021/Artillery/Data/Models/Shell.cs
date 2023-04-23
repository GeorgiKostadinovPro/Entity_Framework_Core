using Artillery.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.Data.Models
{
    public class Shell
    {
        public Shell() 
        { 
            this.Guns = new HashSet<Gun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public double ShellWeight { get; set; }

        [Required]
        [MaxLength(ValidationConstants.ShellCaliberMaxLength)]
        public string Caliber { get; set; } = null!;

        public virtual ICollection<Gun> Guns { get; set; }
    }
}
