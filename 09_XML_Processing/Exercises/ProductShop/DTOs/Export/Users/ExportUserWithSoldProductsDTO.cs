using ProductShop.DTOs.Export.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.Users
{
    [XmlType("User")]
    public class ExportUserWithSoldProductsDTO
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; } = null!;

        [XmlElement("lastName")]
        public string LastName { get; set; } = null!;

        [XmlArray("soldProducts")]
        public ExportProductDTO[] SoldProducts { get; set; }
    }
}
