using System.Collections.Generic;

namespace GetSocialSdk.Editor.Android.Manifest
{
    public class Receiver : AndroidManifestNode
    {
        public Receiver(string name, List<IntentFilter> intentFilters) : base(
            "receiver", 
            ApplicationTag, 
            new Dictionary<string, string>{{NameAttribute, name}})
        {
            intentFilters.ForEach(AddChild);
        }

        public override string ToString()
        {
            return string.Format("Receiver {0}", Attributes[NameAttribute]);
        }
    }
}