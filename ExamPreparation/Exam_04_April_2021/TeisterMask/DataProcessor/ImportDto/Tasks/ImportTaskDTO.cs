using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TeisterMask.Common;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ImportDto.Tasks
{
    [XmlType("Task")]
    public class ImportTaskDTO
    {
        [XmlElement(nameof(Name))]
        [Required]
        [StringLength(ValidationConstants.TaskNameMaxLength,
            MinimumLength = ValidationConstants.TaskNameMinLength)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(OpenDate))]
        [Required]
        public string OpenDate { get; set; } = null!;

        [XmlElement(nameof(DueDate))]
        [Required]
        public string DueDate { get; set; } = null!;

        [XmlElement(nameof(ExecutionType))]
        [Required]
        [Range(ValidationConstants.TaskExecutionTypeMinValue, ValidationConstants.TaskExecutionTypeMaxValue)]
        public int ExecutionType { get; set; }

        [XmlElement(nameof(LabelType))]
        [Required]
        [Range(ValidationConstants.TaskLabelTypeMinValue, ValidationConstants.TaskLabelTypeMaxValue)]
        public int LabelType { get; set; }
    }
}
