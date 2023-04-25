using Boardgames.Common;
using Boardgames.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto.Boardgames
{
    [XmlType("Boardgame")]
    public class ImportBoardgameDTO
    {
        [XmlElement(nameof(Name))]
        [Required]
        [StringLength(ValidationConstants.BoardgameNameMaxLength,
            MinimumLength = ValidationConstants.BoardgameNameMinLength)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(Rating))]
        [Required]
        [Range(ValidationConstants.BoardgameRatingMinValue, ValidationConstants.BoardgameRatingMaxValue)]
        public double Rating { get; set; }

        [XmlElement(nameof(YearPublished))]
        [Required]
        [Range(ValidationConstants.BoardgameYearPublishedMinValue, ValidationConstants.BoardgameYearPublishedMaxValue)]
        public int YearPublished { get; set; }

        [XmlElement(nameof(CategoryType))]
        [Required]
        [Range(ValidationConstants.BoardgameCategoryTypeMinValue, ValidationConstants.BoardgameCategoryTypeMaxValue)]
        public int CategoryType { get; set; }

        [XmlElement(nameof(Mechanics))]
        [Required]
        public string Mechanics { get; set; } = null!;
    }
}
