using System.Collections.Generic;

namespace GetSocialSdk.Editor.Android.Manifest
{
    public class Action : AndroidManifestNode
    {
        public Action(string name) : base("action", IntentFilterTag, new Dictionary<string, string> { { NameAttribute, name } })
        {
        }
    }
}