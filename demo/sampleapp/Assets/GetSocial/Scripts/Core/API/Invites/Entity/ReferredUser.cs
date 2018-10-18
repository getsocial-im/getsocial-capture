using System;
using System.Collections.Generic;
#if UNITY_ANDROID
using UnityEngine;
#endif

namespace GetSocialSdk.Core
{
    public sealed class ReferredUser : PublicUser, IConvertableFromNative<ReferredUser>
    {

        /// <summary>
        /// Date of installation.
        /// </summary>
        /// <value>Date of installation.</value>
        public DateTime InstallationDate { get; private set; }

        
        /// <summary>
        /// One of the channels listed in <see cref="InviteChannelIds"/>.
        /// </summary>
        /// <value>Installation channel.</value>
        public string InstallationChannel { get; private set; }

        public ReferredUser()
        {
            
        }

        internal ReferredUser(Dictionary<string, string> publicProperties, string id, string displayName, string avatarUrl, Dictionary<string, string> identities, DateTime installationDate, string installationChannel) : base(publicProperties, id, displayName, avatarUrl, identities)
        {
            InstallationDate = installationDate;
            InstallationChannel = installationChannel;
        }

        public override string ToString()
        {
            return string.Format("[ReferredUser: Id={0}, DisplayName={1}, Identities={2}, InstallationDate={3}, InstallationChannel={4}]", Id, DisplayName, Identities.ToDebugString(), InstallationDate, InstallationChannel);
        }

        private bool Equals(ReferredUser other)
        {
            return base.Equals(other) && InstallationDate.Equals(other.InstallationDate) && string.Equals(InstallationChannel, other.InstallationChannel);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is ReferredUser && Equals((ReferredUser) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ InstallationDate.GetHashCode();
                hashCode = (hashCode * 397) ^ (InstallationChannel != null ? InstallationChannel.GetHashCode() : 0);
                return hashCode;
            }
        }

#if UNITY_ANDROID
        public new ReferredUser ParseFromAJO(AndroidJavaObject ajo)
        {
            if (ajo.IsJavaNull())
            {
                return null;
            }

            using (ajo)
            {
                base.ParseFromAJO(ajo);                 
                InstallationDate = DateUtils.FromUnixTime(ajo.CallLong("getInstallationDate"));
                InstallationChannel = ajo.CallStr("getInstallationChannel");
            }
            return this;
        }
#elif UNITY_IOS
        public new ReferredUser ParseFromJson(Dictionary<string, object> json)
        {
            base.ParseFromJson(json);
            InstallationDate = DateUtils.FromUnixTime((long) json["InstallationDate"]);
            InstallationChannel = (string) json["InstallationChannel"];
            return this;
        }
#endif
    }
}