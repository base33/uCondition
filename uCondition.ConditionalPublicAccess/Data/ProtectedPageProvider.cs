namespace uCondition.ConditionalPublicAccess.Data
{
    //    public class ProtectedPageProvider
    //    {
    //        public ProtectedPages Load()
    //        {
    //            var path = HttpContext.Current.Server.MapPath("~/app_data/protectedpages.xml");
    //
    //            if (!File.Exists(path))
    //                return new ProtectedPages();
    //
    //            var xml = File.ReadAllText(path);
    //            var toDeserialize = new XmlDocument();
    //            toDeserialize.LoadXml(xml);
    //            return XmlSerialiser.Deserialize<ProtectedPages>(toDeserialize);
    //        }
    //
    //        public void Save(ProtectedPages protectedPage)
    //        {
    //            File.WriteAllText(
    //                HttpContext.Current.Server.MapPath("~/app_data/protectedpages.xml"), XmlSerialiser.Serialize(protectedPage).OuterXml);
    //        }
    //
    //        public ProtectedPage LoadForPath(IEnumerable<int> path)
    //        {
    //            return Load()
    //                .Pages
    //                .Where(c => new[] { c.NodeId }.Intersect(path).Any())
    //                .OrderByDescending(c => path.IndexOf<int>(c.NodeId))
    //                .FirstOrDefault();
    //        }
    //    }
}