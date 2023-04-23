using Artillery.Data.Models.Enums;
using Artillery.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artillery.DataProcessor.ImportDto.Countries;
using Artillery.Common;

namespace Artillery.DataProcessor.ImportDto.Guns
{
    [JsonObject]
    public class ImportGunDTO
    {
        [JsonProperty(nameof(GunWeight))]
        [Required]
        [Range(ValidationConstants.GunWeightMinValue, ValidationConstants.GunWeightMaxValue)]
        public int GunWeight { get; set; }

        [JsonProperty(nameof(BarrelLength))]
        [Required]
        [Range(ValidationConstants.GunBarrelMinLength, ValidationConstants.GunBarrelMaxLength)]
        public double BarrelLength { get; set; }

        [JsonProperty(nameof(NumberBuild))]
        public int? NumberBuild { get; set; }

        [Required]
        [JsonProperty(nameof(Range))]
        [Range(ValidationConstants.GunRangeMinValue, ValidationConstants.GunRangeMaxValue)]
        public int Range { get; set; }

        [Required]
        [JsonProperty(nameof(GunType))]
        public string GunType { get; set; } = null!;

        [Required]
        [JsonProperty(nameof(ManufacturerId))]
        public int ManufacturerId { get; set; }

        [Required]
        [JsonProperty(nameof(ShellId))]
        public int ShellId { get; set; }

        [JsonProperty("Countries")]
        public ImportCountryIdDTO[] CountryIds { get; set; }
    }
}
