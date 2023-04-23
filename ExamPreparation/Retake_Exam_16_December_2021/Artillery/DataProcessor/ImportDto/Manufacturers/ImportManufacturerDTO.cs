using Artillery.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto.Manufacturers
{
    [XmlType("Manufacturer")]
    public class ImportManufacturerDTO
    {
        [XmlElement(nameof(ManufacturerName))]
        [Required]
        [StringLength(ValidationConstants.ManufacturerNameMaxLength,
            MinimumLength = ValidationConstants.ManufacturerNameMinLength)]
        public string ManufacturerName { get; set; } = null!;

        [XmlElement(nameof(Founded))]
        [Required]
        [StringLength(ValidationConstants.ManufacturerFoundedMaxLength,
            MinimumLength = ValidationConstants.ManufacturerFoundedMinLength)]
        public string Founded { get; set; } = null!;
    }
}
