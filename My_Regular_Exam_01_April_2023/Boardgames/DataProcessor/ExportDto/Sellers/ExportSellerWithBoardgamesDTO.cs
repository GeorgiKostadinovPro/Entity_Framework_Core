using Boardgames.DataProcessor.ExportDto.Boardgames;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.DataProcessor.ExportDto.Sellers
{
    [JsonObject]
    public class ExportSellerWithBoardgamesDTO
    {
        [JsonProperty(nameof(Name))]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(Website))]
        public string Website { get; set; } = null!;

        [JsonProperty(nameof(Boardgames))]
        public ExportBoardgameForSellerDTO[] Boardgames { get; set; }
    }
}
