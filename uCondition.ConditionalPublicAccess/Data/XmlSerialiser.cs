using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace uCondition.ConditionalPublicAccess.Data
{
    public static class XmlSerialiser
    {
        public static XmlDocument Serialize<T>(T toSerialize)
        {
            return Serialize<T>(toSerialize, null);
        }

        public static XmlDocument Serialize<T>(
          T toSerialize,
          XmlRootAttribute defaultNamespace)
        {
            var xmlDocument = new XmlDocument();
            var xmlSerializer = defaultNamespace == null ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), defaultNamespace);
            using (var memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, (object)toSerialize);
                memoryStream.Seek(0L, SeekOrigin.Begin);
                xmlDocument.Load((Stream)memoryStream);
                return xmlDocument;
            }
        }

        public static T Deserialize<T>(XmlDocument toDeserialize)
        {
            return Deserialize<T>(toDeserialize, (XmlRootAttribute)null);
        }

        public static T Deserialize<T>(XmlDocument toDeserialize, XmlRootAttribute defaultNamespace)
        {
            var xmlSerializer = defaultNamespace == null ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), defaultNamespace);
            var memoryStream = new MemoryStream();
            toDeserialize.Save(memoryStream);
            memoryStream.Seek(0L, SeekOrigin.Begin);
            return (T)xmlSerializer.Deserialize(memoryStream);
        }
    }
}
