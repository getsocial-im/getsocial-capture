using System.Collections.Generic;

namespace GetSocialSdk.Editor.Android.Manifest
{
    public class MetaData : AndroidManifestNode
    {
        public MetaData(string name, string value) : base(
            "meta-data",
            ApplicationTag,
            new Dictionary<string, string> {{NameAttribute, name}, {ValueAttribute, value}})
        {
        }

        public override string ToString()
        {
            return string.Format("Meta data {0}", Attributes[NameAttribute]);
        }
    }
}