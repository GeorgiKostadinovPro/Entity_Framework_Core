using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.Users
{
    public class ExportUsersGeneralInfoDTO
    {
        [XmlElement("count")]
        public int UsersCount { get; set; }

        [XmlArray("users")]
        public ExportUserAndProductsDTO[] Users { get; set; }
    }
}
