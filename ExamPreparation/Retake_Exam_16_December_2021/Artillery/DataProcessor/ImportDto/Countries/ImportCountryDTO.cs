using Artillery.Common;
using Artillery.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto.Countries
{
    [XmlType("Country")]
    public class ImportCountryDTO
    {
        [XmlElement(nameof(CountryName))]
        [Required]
        [StringLength(ValidationConstants.CountryNameMaxLength,
            MinimumLength = ValidationConstants.CountryNameMinLength)]
        public string CountryName { get; set; } = null!;

        [XmlElement(nameof(ArmySize))]
        [Required]
        [Range(ValidationConstants.CountryArmySizeMinValue, ValidationConstants.CountryArmySizeMaxValue)]
        public int ArmySize { get; set; }
    }
}
