using Boardgames.Common;
using Boardgames.DataProcessor.ImportDto.Boardgames;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto.Creators
{
    [XmlType("Creator")]
    public class ImportCreatorDTO
    {
        [XmlElement(nameof(FirstName))]
        [Required]
        [StringLength(ValidationConstants.CreatorFirstNameMaxLength,
            MinimumLength = ValidationConstants.CreatorFirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [XmlElement(nameof(LastName))]
        [Required]
        [StringLength(ValidationConstants.CreatorLastNameMaxLength,
            MinimumLength = ValidationConstants.CreatorLastNameMinLength)]
        public string LastName { get; set; } = null!;

        [XmlArray(nameof(Boardgames))]
        public ImportBoardgameDTO[] Boardgames { get; set; }  
    }
}
