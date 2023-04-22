using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Theatre.DataProcessor.ExportDto.Casts;

namespace Theatre.DataProcessor.ExportDto.Plays
{
    [XmlType("Play")]
    public class ExportPlayDTO
    {
        [XmlAttribute(nameof(Title))]
        public string Title { get; set; }

        [XmlAttribute(nameof(Duration))]
        public string Duration { get; set; }

        [XmlAttribute(nameof(Rating))]
        public string Rating { get; set; }

        [XmlAttribute(nameof(Genre))]
        public string Genre { get; set; }

        [XmlArray(nameof(Actors))]
        public ExportCastDTO[] Actors { get; set; }
    }
}
