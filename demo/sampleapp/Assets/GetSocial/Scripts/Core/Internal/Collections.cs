using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GetSocialSdk.Core
{
    public static class Collections
    {
        public static bool DictionaryEquals<TKey, TValue>(this Dictionary<TKey, TValue> self,
            Dictionary<TKey, TValue> other)
        {
            return self.Count == other.Count && !self.Except(other).Any();
        }

        public static bool ListEquals<T>(this List<T> self, List<T> other)
        {
            if (self.Count != other.Count)
            {
                return false;
            }

            return !self.Where((t, i) => !Equals(t, other[i])).Any();
        }

        public static bool Texture2DEquals(this Texture2D self, Texture2D other)
        {
            if (self == other)
            {
                return true;
            }

            if (self == null || other == null)
            {
                return false;
            }

            return ListEquals(self.GetPixels().ToList(), other.GetPixels().ToList());
        }
    }
}
