using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Theatre.Data.Models;
using System.Xml.Linq;
using Theatre.Common;

namespace Theatre.DataProcessor.ImportDto.Tickets
{
    [JsonObject]
    public class ImportTicketDTO
    {
        [JsonProperty(nameof(Price))]
        [Required]
        [Range((double)ValidationConstants.TicketPriceMinValue, (double)ValidationConstants.TicketPriceMaxValue)]
        public decimal Price { get; set; }

        [JsonProperty(nameof(RowNumber))]
        [Required]
        [Range(ValidationConstants.TicketRowNumberMinValue, ValidationConstants.TicketRowNumberMaxValue)]
        public sbyte RowNumber { get; set; }

        [JsonProperty(nameof(PlayId))]
        public int PlayId { get; set; }
    }
}
