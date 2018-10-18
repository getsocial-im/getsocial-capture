namespace GetSocialSdk.Editor.Android.Manifest
{
    public interface IFindCriteria<in T>
    {
        bool SatisfiesCriteria(T obj);
    }
}