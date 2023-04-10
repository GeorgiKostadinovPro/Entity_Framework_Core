using Newtonsoft.Json;

namespace CarDealer.DTOs.Export.Cars
{
    [JsonObject]
    public class ExportCarFromMakeToyotaDTO
    {
        public int Id { get; set; }

        public string Make { get; set; } = null!;

        public string Model { get; set; } = null!;

        public long TraveledDistance { get; set; }
    }
}
