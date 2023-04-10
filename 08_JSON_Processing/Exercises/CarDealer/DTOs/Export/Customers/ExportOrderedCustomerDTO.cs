using Newtonsoft.Json;

namespace CarDealer.DTOs.Export.Customers
{
    [JsonObject]
    public class ExportOrderedCustomerDTO
    {
        public string Name { get; set; } = null!;

        public string BirthDate { get; set; }

        public bool IsYoungDriver { get; set; }
    }
}
