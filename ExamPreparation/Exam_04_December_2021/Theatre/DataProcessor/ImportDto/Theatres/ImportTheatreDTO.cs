using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Theatre.Common;
using Theatre.Data.Models;
using Theatre.DataProcessor.ImportDto.Tickets;

namespace Theatre.DataProcessor.ImportDto.Theatres
{
    [JsonObject]
    public class ImportTheatreDTO
    {
        [JsonProperty(nameof(Name))]
        [Required]
        [StringLength(ValidationConstants.TheatreNameMaxLength,
            MinimumLength = ValidationConstants.TheatreNameMinLength)]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(NumberOfHalls))]
        [Required]
        [Range(ValidationConstants.TheatreNumberOfHallsMinValue, ValidationConstants.TheatreNumberOfHallsMaxValue)]
        public sbyte NumberOfHalls { get; set; }

        [JsonProperty(nameof(Director))]
        [Required]
        [StringLength(ValidationConstants.TheatreDirectorMaxLength,
            MinimumLength = ValidationConstants.TheatreDirectorMinLength)]
        public string Director { get; set; } = null!;

        [JsonProperty(nameof(Tickets))]
        public ImportTicketDTO[] Tickets { get; set; }
    }
}
