﻿using System.Xml.Serialization;

namespace CarDealer.DTOs.Import.Suppliers
{
    [XmlType("Supplier")]
    public class ImportSupplierDTO
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("isImporter")]
        public bool IsImporter { get; set; }
    }
}