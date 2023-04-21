using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto.Tasks
{
    [XmlType("Task")]
    public class ExportTaskForProjectDTO
    {
        [XmlElement(nameof(Name))]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(Label))]
        public string Label { get; set; } = null!;
    }
}
