using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Theatre.Common;
using Theatre.Data.Models;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto.Cars
{
    [XmlType("Cast")]
    public class ImportCastDTO
    {
        [XmlElement(nameof(FullName))]
        [Required]
        [StringLength(ValidationConstants.CastFullNameMaxLength,
            MinimumLength = ValidationConstants.CastFullNameMinLength)]
        public string FullName { get; set; } = null!;

        [XmlElement(nameof(IsMainCharacter))]
        [Required]
        public bool IsMainCharacter { get; set; }

        [XmlElement(nameof(PhoneNumber))]
        [Required]
        [RegularExpression(@"^\+44-[0-9]{2}-[0-9]{3}-[0-9]{4}$")]
        public string PhoneNumber { get; set; } = null!;

        [XmlElement(nameof(PlayId))]
        public int PlayId { get; set; }
    }
}
