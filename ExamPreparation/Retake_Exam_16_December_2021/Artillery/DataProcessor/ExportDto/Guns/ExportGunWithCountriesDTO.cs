using Artillery.DataProcessor.ExportDto.Countires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto.Guns
{
    [XmlType("Gun")]
    public class ExportGunWithCountriesDTO
    {
        [XmlAttribute(nameof(Manufacturer))]
        public string Manufacturer { get; set; } = null!;

        [XmlAttribute(nameof(GunType))]
        public string GunType { get; set; } = null!;

        [XmlAttribute(nameof(GunWeight))]
        public int GunWeight { get; set; }

        [XmlAttribute(nameof(BarrelLength))]
        public double BarrelLength { get; set; }

        [XmlAttribute(nameof(Range))]
        public int Range { get; set; }

        [XmlArray(nameof(Countries))]
        public ExportCountryDTO[] Countries { get; set; }
    }
}
