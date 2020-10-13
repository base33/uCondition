using System.Collections.Generic;
using System.Xml.Serialization;

namespace uCondition.ConditionalPublicAccess.Data
{
    [XmlRoot("ProtectedPages")]
    public class ProtectedPages
    {
        [XmlAttribute("Version")]
        public int Version { get; set; } = 1;

        public List<ProtectedPage> Pages { get; set; } = new List<ProtectedPage>();
    }
}
