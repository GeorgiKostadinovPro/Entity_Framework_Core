using Boardgames.DataProcessor.ExportDto.Boardgames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto.Creators
{
    [XmlType("Creator")]
    public class ExportCreatorWithBoardgamesDTO
    {
        [XmlAttribute(nameof(BoardgamesCount))]
        public int BoardgamesCount { get; set; }

        [XmlElement(nameof(CreatorName))]
        public string CreatorName { get; set; } = null!;

        [XmlArray(nameof(Boardgames))]
        public ExportBoardgameForCreatorDTO[] Boardgames { get; set; }
    }
}
