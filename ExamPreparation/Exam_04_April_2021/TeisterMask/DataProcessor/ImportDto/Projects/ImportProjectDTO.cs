using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TeisterMask.Common;
using TeisterMask.DataProcessor.ImportDto.Tasks;

namespace TeisterMask.DataProcessor.ImportDto.Projects
{
    [XmlType("Project")]
    public class ImportProjectDTO
    {
        [XmlElement(nameof(Name))]
        [Required]
        [StringLength(ValidationConstants.ProjectNameMaxLength,
            MinimumLength = ValidationConstants.ProjectNameMinLength)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(OpenDate))]
        [Required]
        public string OpenDate { get; set; } = null!;

        [XmlElement(nameof(DueDate))]
        public string? DueDate { get; set; }

        [XmlArray(nameof(Tasks))]
        public ImportTaskDTO[] Tasks { get; set; }
    }
}
