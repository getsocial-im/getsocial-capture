using System;

namespace GetSocialSdk.Editor
{
    public class SemanticVersion : IComparable, IComparable<SemanticVersion>
    {
        public int Major { get; private set; }
        public int Minor { get; private set; }
        public int Bugfix { get; private set; }

        public SemanticVersion(int major, int minor, int bugfix)
        {
            Major = major;
            Minor = minor;
            Bugfix = bugfix;
        }

        public static SemanticVersion Parse(string stringVersion)
        {
            var parts = stringVersion.Split('.');
            return new SemanticVersion(Int32.Parse(parts[0]), Int32.Parse(parts[1]), Int32.Parse(parts[2]));
        }

        protected bool Equals(SemanticVersion other)
        {
            return Major == other.Major && Minor == other.Minor && Bugfix == other.Bugfix;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SemanticVersion) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Major;
                hashCode = (hashCode * 397) ^ Minor;
                hashCode = (hashCode * 397) ^ Bugfix;
                return hashCode;
            }
        }

        public int CompareTo(SemanticVersion otherVersion)
        {
            if (ReferenceEquals(this, otherVersion)) return 0;
            if (ReferenceEquals(null, otherVersion)) return 1;
            var majorComparison = Major.CompareTo(otherVersion.Major);
            if (majorComparison != 0) return majorComparison;
            var minorComparison = Minor.CompareTo(otherVersion.Minor);
            if (minorComparison != 0) return minorComparison;
            return Bugfix.CompareTo(otherVersion.Bugfix);
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as SemanticVersion);
        }

        public static bool operator ==(SemanticVersion left, SemanticVersion right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }

        public static bool operator !=(SemanticVersion left, SemanticVersion right)
        {
            return !(left == right);
        }

        public static bool operator <(SemanticVersion left, SemanticVersion right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(SemanticVersion left, SemanticVersion right)
        {
            return left.CompareTo(right) > 0;
        }
    }
}