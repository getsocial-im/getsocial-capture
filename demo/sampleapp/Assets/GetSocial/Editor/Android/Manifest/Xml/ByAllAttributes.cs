using System;
using System.Linq;
using System.Xml;

namespace GetSocialSdk.Editor.Android.Manifest
{
    public class ByAllAttributes : IFindCriteria<XmlNode>
    {
        private readonly AndroidManifestNode _node;

        public ByAllAttributes(AndroidManifestNode node)
        {
            _node = node;
        }

        public bool SatisfiesCriteria(XmlNode xmlNode)
        {
            if (xmlNode != null)
            {
                if (xmlNode.Name.Equals(_node.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (xmlNode.Attributes != null && xmlNode.Attributes.Count == _node.Attributes.Count)
                    {
                        return _node.Attributes.All(attribute =>
                        {
                            var attributeNode = xmlNode.Attributes.GetNamedItem(attribute.Key);
                            return attributeNode != null && attributeNode.Value.Equals(attribute.Value);
                        });
                    }
                }
            }

            return false;
        }
    }
}