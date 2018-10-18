namespace GetSocialSdk.Core
{
    public static class AssetStoreUtils
    {
        public static bool IsAssetStorePackage()
        {
            return string.Equals(BuildConfig.PublishTarget, "unity-asset-store");
        }
    }
}