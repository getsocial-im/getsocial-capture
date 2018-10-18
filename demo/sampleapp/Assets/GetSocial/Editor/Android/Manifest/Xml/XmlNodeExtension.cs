using System.Xml;

namespace GetSocialSdk.Editor.Android.Manifest
{
    public static class XmlNodeExtension
    {
        #region public methods

        public static void InsertAt(this XmlNode node, XmlNode insertingNode, int index = 0)
        {
            if (insertingNode == null)
            {
                return;
            }
            if (index < 0)
            {
                index = 0;
            }

            var childNodes = node.ChildNodes;
            var childrenCount = childNodes.Count;

            if (index >= childrenCount)
            {
                node.AppendChild(insertingNode);
                return;
            }

            var followingNode = childNodes[index];

            node.InsertBefore(insertingNode, followingNode);
        }
        
        public static XmlNode FindNodeRecursive(this XmlNode root, IFindCriteria<XmlNode> criteria)
        {
            if (criteria.SatisfiesCriteria(root))
            {
                return root;
            }
             
            XmlNode currentNode = root.FirstChild;
            while (currentNode != null)
            {
                var node = currentNode.FindNodeRecursive(criteria);
                if(node != null)
                {
                    return node;
                }
                currentNode = currentNode.NextSibling;
            }   
            
            return null;
        }

        public static XmlNode FindNodeInChildren(this XmlNode root, IFindCriteria<XmlNode> criteria)
        {
            XmlNode currentNode = root.FirstChild;
            while (currentNode != null)
            {
                if (criteria.SatisfiesCriteria(currentNode))
                {
                    return currentNode;
                }
                currentNode = currentNode.NextSibling;
            }
            return null;
        }

        public static void AddAndroidManifestNode(this XmlNode root, XmlDocument xmlDocument, AndroidManifestNode node)
        {
            var xmlElement = xmlDocument.CreateElement(node.Name);

            foreach (var attribute in node.Attributes)
            {
                var split = attribute.Key.Split(':');
                var prefix = split[0];
                var name = split[1];
                
                var xmlNamespace = xmlDocument.FindNodeInChildren(new ByName(AndroidManifestNode.ManifestTag)).GetNamespaceOfPrefix(prefix);

                xmlElement.SetAttribute(name, xmlNamespace, attribute.Value);
            };
            
            node.ChildNodes.ForEach(childNode => xmlElement.AddAndroidManifestNode(xmlDocument, childNode));
            
            // always add our elements on top
            root.InsertAt(xmlElement, 0);
        }

        #endregion
    }
}