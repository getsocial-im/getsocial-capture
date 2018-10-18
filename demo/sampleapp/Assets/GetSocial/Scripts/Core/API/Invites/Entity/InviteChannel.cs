using System;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using System.Collections.Generic;
#endif

namespace GetSocialSdk.Core
{
    /// <summary>
    ///  Stores information about a way to send an invite and how it should be presented in a list.
    /// </summary>
    public sealed class InviteChannel : IConvertableFromNative<InviteChannel>
    {

        /// <summary>
        /// Gets the invite channel identifier.
        /// </summary>
        /// <value>The invite channel identifier.</value>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the invite channel name.
        /// </summary>
        /// <value>The invite channel name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the invite channel icon image URL.
        /// </summary>
        /// <value>The invite channel icon image URL.</value>
        public string IconImageUrl { get; private set; }

        /// <summary>
        /// Gets invite channel the display order as configured on GetSocial Dashboard.
        /// </summary>
        /// <value>The invite channel the display order as configured on GetSocial Dashboard.</value>
        public int DisplayOrder { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this invite channel is enabled on GetSocial Dashboard.
        /// </summary>
        /// <value><c>true</c> if this invite channel is enabled on GetSocial Dashboard; otherwise, <c>false</c>.</value>
        public bool IsEnabled { get; private set; }

        internal InviteChannel(string id, string name, string iconImageUrl, int displayOrder, bool isEnabled)
        {
            Id = id;
            Name = name;
            IconImageUrl = iconImageUrl;
            DisplayOrder = displayOrder;
            IsEnabled = isEnabled;
        }

        public InviteChannel()
        {
            
        }
        
        public override string ToString()
        {
            return
                string.Format(
                    "[InviteChannel: Id={0}, Name={1}, IconImageUrl={2}, DisplayOrder={3}, IsEnabled={4}]",
                    Id, Name, IconImageUrl, DisplayOrder, IsEnabled);
        }

        private bool Equals(InviteChannel other)
        {
            return string.Equals(Id, other.Id) && string.Equals(Name, other.Name) && string.Equals(IconImageUrl, other.IconImageUrl) && DisplayOrder == other.DisplayOrder && IsEnabled == other.IsEnabled;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is InviteChannel && Equals((InviteChannel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (IconImageUrl != null ? IconImageUrl.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ DisplayOrder;
                hashCode = (hashCode * 397) ^ IsEnabled.GetHashCode();
                return hashCode;
            }
        }

#if UNITY_ANDROID
        public InviteChannel ParseFromAJO(AndroidJavaObject ajo)
        {
            JniUtils.CheckIfClassIsCorrect(ajo, "InviteChannel");

            using (ajo)
            {
                Id = ajo.CallStr("getChannelId");
                Name = ajo.CallStr("getChannelName");
                IconImageUrl = ajo.CallStr("getIconImageUrl");
                DisplayOrder = ajo.CallInt("getDisplayOrder");
                IsEnabled = ajo.CallBool("isEnabled");
            }
            return this;
        }
#elif UNITY_IOS
        public InviteChannel ParseFromJson(Dictionary<string, object> jsonDic)
        {
            Id = jsonDic["Id"] as string;
            Name = jsonDic["Name"] as string;
            IconImageUrl = jsonDic["IconImageUrl"] as string;
            DisplayOrder = (int) (long) jsonDic["DisplayOrder"];
            IsEnabled = (bool) jsonDic["IsEnabled"];

            return this;
        }
#endif
    }
}