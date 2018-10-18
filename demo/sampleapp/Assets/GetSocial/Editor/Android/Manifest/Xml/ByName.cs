using System.Xml;

namespace GetSocialSdk.Editor.Android.Manifest
{
    public class ByName : IFindCriteria<XmlNode>
    {
        private readonly string _name;

        public ByName(string name)
        {
            _name = name;
        }

        public bool SatisfiesCriteria(XmlNode xmlNode)
        {
            if (xmlNode == null)
            {
                return false;
            }

            return _name.Equals(xmlNode.Name);
        }
    }
}