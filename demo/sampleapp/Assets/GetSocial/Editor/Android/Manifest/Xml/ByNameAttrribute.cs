using System;
using System.Xml;

namespace GetSocialSdk.Editor.Android.Manifest
{
    public class ByNameAttrribute : IFindCriteria<XmlNode>
    {
        private readonly AndroidManifestNode _node;

        public ByNameAttrribute(AndroidManifestNode node)
        {
            _node = node;
        }

        public bool SatisfiesCriteria(XmlNode xmlNode)
        {
            if (xmlNode != null)
            {
                if (xmlNode.Name.Equals(_node.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (xmlNode.Attributes != null)
                    {
                        var attributeNode = xmlNode.Attributes.GetNamedItem(AndroidManifestNode.NameAttribute);
                        return attributeNode != null 
                               && _node.Attributes.ContainsKey(AndroidManifestNode.NameAttribute)  
                               && attributeNode.Value.Equals(_node.Attributes[AndroidManifestNode.NameAttribute]);
                    }
                }
            }

            return false;
        }
    }
}