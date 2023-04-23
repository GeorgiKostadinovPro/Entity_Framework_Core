using Artillery.Common;
using Artillery.DataProcessor.ExportDto.Guns;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto.Shells
{
    [JsonObject]
    public class ExportShellWithGunsDTO
    {
        [JsonProperty(nameof(ShellWeight))]
        public double ShellWeight { get; set; }

        [JsonProperty(nameof(Caliber))]
        public string Caliber { get; set; } = null!;

        [JsonProperty(nameof(Guns))]
        public ExportSingleGunDTO[] Guns { get; set; }
    }
}
