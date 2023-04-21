using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ExportDto.Tasks
{
    [JsonObject]
    public class ExportTaskForEmployeeDTO
    {
        [JsonProperty(nameof(TaskName))]
        public string TaskName { get; set; } = null!;

        [JsonProperty(nameof(OpenDate))]
        public string OpenDate { get; set; } = null!;

        [JsonProperty(nameof(DueDate))]
        public string DueDate { get; set; } = null!;

        [JsonProperty(nameof(LabelType))]
        public string LabelType { get; set; } = null!;

        [JsonProperty(nameof(ExecutionType))]
        public string ExecutionType { get; set; } = null!;
    }
}
