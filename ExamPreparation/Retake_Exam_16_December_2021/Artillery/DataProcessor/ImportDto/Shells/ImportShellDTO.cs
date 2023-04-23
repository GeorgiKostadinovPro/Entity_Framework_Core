using Artillery.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto.Shells
{

    [XmlType("Shell")]
    public class ImportShellDTO
    {
        [XmlElement(nameof(ShellWeight))]
        [Required]
        [Range(ValidationConstants.ShellWeightMinValue, ValidationConstants.ShellWeightMaxValue)]
        public double ShellWeight { get; set; }

        [XmlElement(nameof(Caliber))]
        [Required]
        [StringLength(ValidationConstants.ShellCaliberMaxLength, 
            MinimumLength = ValidationConstants.ShellCaliberMinLength)]
        public string Caliber { get; set; } = null!;
    }
}
