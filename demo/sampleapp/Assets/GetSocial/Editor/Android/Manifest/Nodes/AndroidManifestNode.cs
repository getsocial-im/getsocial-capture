using System.Collections.Generic;

namespace GetSocialSdk.Editor.Android.Manifest
{
    public class AndroidManifestNode
    {
        public const string ApplicationTag = "application";
        public const string ManifestTag = "manifest";
        public const string IntentFilterTag = "intent-filter";
        
        public const string NameAttribute = "android:name";
        public const string ValueAttribute = "android:value";
        
        public string Name { get; private set; }
        public string ParentName { get; private set; }
        
        public List<AndroidManifestNode> ChildNodes { get; private set; }
        public Dictionary<string, string> Attributes { get; private set; }

        public AndroidManifestNode(string name, string parentName = "", Dictionary<string, string> attributes = null, List<AndroidManifestNode> childNodes = null)
        {
            Name = name;
            ParentName = parentName;
            Attributes = attributes ?? new Dictionary<string, string>();
            ChildNodes = childNodes ?? new List<AndroidManifestNode>();
        }

        public void AddChild(AndroidManifestNode childNode)
        {
            ChildNodes.Add(childNode);
        }
        
        public override string ToString()
        {
            return string.Format("Tag {0}", Name);
        }
    }
}