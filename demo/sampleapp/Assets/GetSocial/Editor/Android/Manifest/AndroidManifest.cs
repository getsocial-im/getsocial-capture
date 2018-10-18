using System;
using System.Xml;

namespace GetSocialSdk.Editor.Android.Manifest
{
    public class AndroidManifest
    {
        #region fields
        
        private readonly string _path;
        private readonly XmlDocument _xmlDocument;
        private readonly XmlNode _applicationNode;

        #endregion
        
        
        #region public api
        
        public AndroidManifest(string path)
        {
            _path = path;
            
            _xmlDocument = new XmlDocument();
            _xmlDocument.Load(path);
            
            _applicationNode = _xmlDocument.FindNodeRecursive(new ByName("application"));
            
            if (_applicationNode == null)
            {
                throw new ArgumentException("Failed to parse AndroidManifest with path " + path);
            }
        }

        public bool Contains(AndroidManifestNode node)
        {
            var parentNode = _xmlDocument.FindNodeRecursive(new ByName(node.ParentName));
            if (parentNode != null)
            {
                var xmlNode = parentNode.FindNodeInChildren(new ByAllAttributes(node));
                return xmlNode != null;
            }
            return false;
        }

        public void RemoveSimilar(AndroidManifestNode node)
        {
            var parentNode = _xmlDocument.FindNodeRecursive(new ByName(node.ParentName));
            if (parentNode != null)
            {
                var xmlNode = parentNode.FindNodeInChildren(new ByNameAttrribute(node));
                if (xmlNode != null)
                {
                    parentNode.RemoveChild(xmlNode);
                }
            }
        }

        public void Add(AndroidManifestNode node)
        {
            var parentNode = _xmlDocument.FindNodeRecursive(new ByName(node.ParentName));
            if (parentNode != null)
            {
                parentNode.AddAndroidManifestNode(_xmlDocument, node);
            }
            else
            {
                throw new Exception("GetSocial: Failed to modify AndroidManifest.xml. Can't find node " + node.ParentName);
            }
        }

        public void Save()
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(_path, settings))
            {
                _xmlDocument.Save(xmlWriter);
            }
        }
        
        #endregion


        #region private methods

          

        #endregion
    }
}