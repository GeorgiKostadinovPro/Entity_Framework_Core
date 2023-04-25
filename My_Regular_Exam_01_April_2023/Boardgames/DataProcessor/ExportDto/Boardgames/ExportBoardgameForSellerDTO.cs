using Boardgames.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto.Boardgames
{
    [JsonObject]
    public class ExportBoardgameForSellerDTO
    {
        [JsonProperty(nameof(Name))]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(Rating))]
        public double Rating { get; set; }

        [JsonProperty(nameof(Mechanics))]
        public string Mechanics { get; set; } = null!;

        [JsonProperty(nameof(Category))]
        public string Category { get; set; } = null!;
    }
}
