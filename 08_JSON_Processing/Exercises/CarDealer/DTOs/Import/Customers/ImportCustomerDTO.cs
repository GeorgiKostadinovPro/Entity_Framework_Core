﻿using Newtonsoft.Json;

namespace CarDealer.DTOs.Import.Customers
{
    [JsonObject]
    public class ImportCustomerDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("isYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}
