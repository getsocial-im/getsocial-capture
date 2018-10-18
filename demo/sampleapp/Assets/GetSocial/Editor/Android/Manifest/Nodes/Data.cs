using System.Collections.Generic;

namespace GetSocialSdk.Editor.Android.Manifest
{
    public class Data : AndroidManifestNode
    {
        public Data(string scheme, string host) : base(
            "data", 
            IntentFilterTag, 
            new Dictionary<string, string> {
                { "android:scheme", scheme },
                { "android:host", host }
            })
        {
        }
    }
}