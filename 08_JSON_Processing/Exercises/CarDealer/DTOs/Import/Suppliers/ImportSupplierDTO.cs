﻿using Newtonsoft.Json;

namespace CarDealer.DTOs.Import.Suppliers
{
    [JsonObject]
    public class ImportSupplierDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("isImporter")]
        public bool IsImporter { get; set; }
    }
}
