using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery.DataProcessor.ImportDto.Countries
{
    [JsonObject]
    public class ImportCountryIdDTO
    {
        [JsonProperty(nameof(Id))]
        [Required]
        public int Id { get; set; }
    }
}
