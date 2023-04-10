using CarDealer.DTOs.Export.Parts;
using Newtonsoft.Json;

namespace CarDealer.DTOs.Export.Cars
{
    [JsonObject]
    public class ExportCarWithItsListOfPartsDTO
    {
        [JsonProperty("car")]
        public ExportCarDTO Car { get; set; } = null!;

        [JsonProperty("parts")]
        public ExportPartDTO[] CarParts { get; set; } = null!;
    }
}
