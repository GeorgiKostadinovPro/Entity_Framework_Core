using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeisterMask.DataProcessor.ExportDto.Tasks;

namespace TeisterMask.DataProcessor.ExportDto.Employees
{
    [JsonObject]
    public class ExportEmployeeDTO
    {
        [JsonProperty(nameof(Username))]
        public string Username { get; set; } = null!;

        [JsonProperty(nameof(Tasks))]
        public ExportTaskForEmployeeDTO[] Tasks { get; set; }
    }
}
