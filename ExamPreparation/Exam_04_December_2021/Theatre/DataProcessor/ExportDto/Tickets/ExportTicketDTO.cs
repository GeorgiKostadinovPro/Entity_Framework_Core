using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Theatre.DataProcessor.ExportDto.Tickets
{
    [JsonObject]
    public class ExportTicketDTO
    {
        [JsonProperty(nameof(Price))]
        public decimal Price { get; set; }

        [JsonProperty(nameof(RowNumber))]
        public int RowNumber { get; set; }
    }
}
