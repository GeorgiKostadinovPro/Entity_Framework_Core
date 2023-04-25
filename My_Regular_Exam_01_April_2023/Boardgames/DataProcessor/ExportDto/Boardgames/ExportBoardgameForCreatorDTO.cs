using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto.Boardgames
{
    [XmlType("Boardgame")]
    public class ExportBoardgameForCreatorDTO
    {
        [XmlElement(nameof(BoardgameName))]
        public string BoardgameName { get; set; } = null!;

        [XmlElement(nameof(BoardgameYearPublished))]
        public int BoardgameYearPublished { get; set; }
    }
}
