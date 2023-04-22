using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Theatre.Common;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ImportDto.Plays
{
    [XmlType("Play")]
    public class ImportPlayDTO
    {
        [XmlElement(nameof(Title))]
        [Required]
        [StringLength(ValidationConstants.PlayTitleMaxLength,
        MinimumLength = ValidationConstants.PlayTitleMinLength)]
        public string Title { get; set; } = null!;

        [XmlElement(nameof(Duration))]
        public string Duration { get; set; }

        [XmlElement(nameof(Rating))]
        [Range(ValidationConstants.PlayRatingMinValue, ValidationConstants.PlayRatingMaxValue)]
        public float Rating { get; set; }

        [XmlElement(nameof(Genre))]
        [MaxLength(ValidationConstants.PlayDescriptionMaxLength)]
        public string Genre { get; set; }

        
        [XmlElement(nameof(Description))]
        [Required]
        [MaxLength(ValidationConstants.PlayDescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [XmlElement(nameof(Screenwriter))]
        [Required]
        [StringLength(ValidationConstants.PlayScreenwriterMaxLength,
            MinimumLength = ValidationConstants.PlayScreenwriterMinLength)]
        public string Screenwriter { get; set; } = null!;
    }
}
