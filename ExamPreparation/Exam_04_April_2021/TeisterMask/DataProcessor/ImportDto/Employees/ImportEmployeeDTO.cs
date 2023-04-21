using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeisterMask.Common;

namespace TeisterMask.DataProcessor.ImportDto.Employees
{
    [JsonObject]
    public class ImportEmployeeDTO
    {
        [JsonProperty(nameof(Username))]
        [Required]
        [StringLength(ValidationConstants.EmployeeUsernameMaxLength, 
            MinimumLength = ValidationConstants.EmployeeUsernameMinLength)]
        [RegularExpression("^[A-Za-z0-9]+$")]
        public string Username { get; set; } = null!;

        [JsonProperty(nameof(Email))]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [JsonProperty(nameof(Phone))]
        [Required]
        [RegularExpression("^[0-9]{3}-[0-9]{3}-[0-9]{4}$")]
        public string Phone { get; set; } = null!;

        [JsonProperty("Tasks")]
        public int[] TaskIds { get; set; }
    }
}
