using Newtonsoft.Json;

namespace CarDealer.DTOs.Export.Suppliers
{
    [JsonObject]
    public class ExportLocalSupplierDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int PartsCount { get; set; }
    }
}
