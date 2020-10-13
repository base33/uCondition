using System.Collections.Generic;
using System.Xml.Serialization;

namespace uCondition.ConditionalPublicAccess.Data
{
    public class ProtectedPage : ProtectedPageCondition
    {
        [XmlAttribute]
        public int Id { get; set; }

        public List<ProtectedPageCondition> Conditions { get; set; }
    }
}
