using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TeisterMask.DataProcessor.ExportDto.Tasks;

namespace TeisterMask.DataProcessor.ExportDto.Projects
{
    [XmlType("Project")]
    public class ExportProjectDTO
    {
        [XmlAttribute(nameof(TasksCount))]
        public int TasksCount { get; set; }

        [XmlElement(nameof(ProjectName))]
        public string ProjectName { get; set; } = null!;

        [XmlElement(nameof(HasEndDate))]
        public string HasEndDate { get; set; } = null!;

        [XmlArray(nameof(Tasks))]
        public ExportTaskForProjectDTO[] Tasks { get; set; }
    }
}
