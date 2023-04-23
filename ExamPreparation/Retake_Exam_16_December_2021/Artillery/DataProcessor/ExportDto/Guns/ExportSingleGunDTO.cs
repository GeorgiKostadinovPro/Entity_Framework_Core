using Artillery.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery.DataProcessor.ExportDto.Guns
{
    [JsonObject]
    public class ExportSingleGunDTO
    {
        [JsonProperty(nameof(GunType))]
        public string GunType { get; set; } = null!;

        [JsonProperty(nameof(GunWeight))]
        public int GunWeight { get; set; }

        [JsonProperty(nameof(BarrelLength))]
        public double BarrelLength { get; set; }

        [JsonProperty(nameof(Range))]
        public string Range { get; set; } = null!;
    }
}
