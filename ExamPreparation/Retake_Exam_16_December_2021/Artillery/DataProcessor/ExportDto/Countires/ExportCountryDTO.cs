using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto.Countires
{
    [XmlType("Country")]
    public class ExportCountryDTO
    {
        [XmlAttribute("Country")]
        public string Country { get; set; } = null!;

        [XmlAttribute(nameof(ArmySize))]
        public int ArmySize { get; set; }
    }
}
