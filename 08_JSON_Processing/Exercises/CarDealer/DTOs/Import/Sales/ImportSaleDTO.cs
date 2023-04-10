using Newtonsoft.Json;

namespace CarDealer.DTOs.Import.Sales
{
    [JsonObject]
    public class ImportSaleDTO
    {
        [JsonProperty("discount")]
        public decimal Discount { get; set; }

        [JsonProperty("carId")]
        public int CarId { get; set; }

        [JsonProperty("customerId")]
        public int CustomerId { get; set; }
    }
}
