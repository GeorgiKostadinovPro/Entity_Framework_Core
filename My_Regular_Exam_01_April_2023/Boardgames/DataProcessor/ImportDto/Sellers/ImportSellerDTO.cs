using Boardgames.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.DataProcessor.ImportDto.Sellers
{
    [JsonObject]
    public class ImportSellerDTO
    {
        [JsonProperty(nameof(Name))]
        [Required]
        [StringLength(ValidationConstants.SellerNameMaxLength,
            MinimumLength = ValidationConstants.SellerNameMinLength)]
        public string Name { get; set; } = null!;

        [JsonProperty(nameof(Address))]
        [Required]
        [StringLength(ValidationConstants.SellerAddressMaxLength,
            MinimumLength = ValidationConstants.SellerAddressMinLength)]
        public string Address { get; set; } = null!;

        [JsonProperty(nameof(Country))]
        [Required]
        public string Country { get; set; } = null!;

        [JsonProperty(nameof(Website))]
        [Required]
        [RegularExpression("^www.[A-Za-z0-9-]+.com$")]
        public string Website { get; set; } = null!;

        [JsonProperty("Boardgames")]
        public int[] BoardgameIds { get; set; }
    }
}
