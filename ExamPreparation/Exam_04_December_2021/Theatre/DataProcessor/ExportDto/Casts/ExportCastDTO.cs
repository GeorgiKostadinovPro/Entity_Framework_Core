using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ExportDto.Casts
{
    [XmlType("Actor")]
    public class ExportCastDTO
    {
        [XmlAttribute(nameof(FullName))]
        public string FullName { get; set; }

        [XmlAttribute(nameof(MainCharacter))]
        public string MainCharacter { get; set; }
    }
}
