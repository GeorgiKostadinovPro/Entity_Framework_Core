using CarDealer.DTOs.Export.Cars;
using CarDealer.DTOs.Import.Suppliers;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Utilities
{
    public class XmlHelper
    {
        public T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);

            XmlSerializer xmlSerializer
                = new XmlSerializer(typeof(T), xmlRoot);

            using StringReader reader = new StringReader(inputXml);

            T deserializedDtos =
                (T)xmlSerializer.Deserialize(reader);

            return deserializedDtos;
        }

        public IEnumerable<T> DeserializeCollection<T>(string inputXml, string rootName)
            where T : class, new()
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);

            XmlSerializer xmlSerializer
                = new XmlSerializer(typeof(T[]), xmlRoot);

            using StringReader reader = new StringReader(inputXml);

            T[] deserializedDtos =
                (T[])xmlSerializer.Deserialize(reader);

            return deserializedDtos;
        }

        public string Serialize<T>(T obj, string rootName)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRootAttribute
                = new XmlRootAttribute(rootName);

            XmlSerializer xmlSerializer
                = new XmlSerializer(typeof(T), xmlRootAttribute);

            XmlSerializerNamespaces xmlSerializerNamespaces
                = new XmlSerializerNamespaces();

            xmlSerializerNamespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);

            xmlSerializer.Serialize(writer, obj, xmlSerializerNamespaces);

            return sb.ToString().TrimEnd();
        }

        public string Serialize<T>(T[] obj, string rootName)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRootAttribute
                = new XmlRootAttribute(rootName);

            XmlSerializer xmlSerializer
                = new XmlSerializer(typeof(T[]), xmlRootAttribute);

            XmlSerializerNamespaces xmlSerializerNamespaces
                = new XmlSerializerNamespaces();

            xmlSerializerNamespaces.Add(string.Empty, string.Empty);

            using StringWriter writer = new StringWriter(sb);

            xmlSerializer.Serialize(writer, obj, xmlSerializerNamespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
