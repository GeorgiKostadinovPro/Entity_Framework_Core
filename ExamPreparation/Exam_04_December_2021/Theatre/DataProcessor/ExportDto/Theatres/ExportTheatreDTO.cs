using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Theatre.DataProcessor.ExportDto.Tickets;

namespace Theatre.DataProcessor.ExportDto.Theatres
{
    [JsonObject]
    public class ExportTheatreDTO
    {
        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        [JsonProperty(nameof(Halls))]
        public int Halls { get; set; }

        [JsonProperty(nameof(TotalIncome))]
        public decimal TotalIncome { get; set; }

        [JsonProperty(nameof(Tickets))]
        public ExportTicketDTO[] Tickets { get; set; }
    }
}
