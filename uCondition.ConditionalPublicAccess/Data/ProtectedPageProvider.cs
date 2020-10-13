using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using Umbraco.Core;

namespace uCondition.ConditionalPublicAccess.Data
{
    public class ProtectedPageProvider
    {
        public ProtectedPages Load()
        {
            var path = HttpContext.Current.Server.MapPath("~/app_data/protectedpages.xml");

            if (!File.Exists(path))
                return new ProtectedPages();

            var xml = File.ReadAllText(path);
            var toDeserialize = new XmlDocument();
            toDeserialize.LoadXml(xml);
            return XmlSerialiser.Deserialize<ProtectedPages>(toDeserialize);
        }

        public void Save(ProtectedPages protectedPage)
        {
            File.WriteAllText(HttpContext.Current.Server.MapPath("~/app_data/protectedpages.xml"), XmlSerialiser.Serialize(protectedPage).OuterXml);
        }

        public ProtectedPage LoadForPath(IEnumerable<int> path)
        {
            return Load().Pages.Where(c => new[] { c.Id }.Intersect(path).Any()).OrderByDescending(c => path.IndexOf<int>(c.Id)).FirstOrDefault();
        }
    }
}
