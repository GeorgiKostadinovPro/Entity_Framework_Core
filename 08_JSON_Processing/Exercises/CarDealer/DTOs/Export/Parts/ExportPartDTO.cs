using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DTOs.Export.Parts
{
    [JsonObject]
    public class ExportPartDTO
    {
        public string Name { get; set; } = null!;

        public string Price { get; set; } = null!;
    }
}
