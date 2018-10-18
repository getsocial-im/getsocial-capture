using System.Linq;

namespace GetSocialSdk.Editor
{
    public static class StringExtentions
    {
        public static string Capitalize(this string input)
        {
            if(input == null)
                return string.Empty;

            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}