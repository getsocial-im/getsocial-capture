using System.Collections.Generic;

namespace GetSocialSdk.Editor.Android.Manifest
{
    public class Provider : AndroidManifestNode
    {
        public Provider(string name, string authorities, bool exported, bool enabled = true) : base(
            "provider",
            ApplicationTag,
            new Dictionary<string, string> {
                {NameAttribute, name}, 
                {"android:authorities", authorities}, 
                {"android:exported", exported.ToString().ToLower()},
                {"android:enabled", enabled.ToString().ToLower()}
            }
            )
        {
        }

        public override string ToString()
        {
            return string.Format("Provider {0}", Attributes[NameAttribute]);
        }
    }
}
