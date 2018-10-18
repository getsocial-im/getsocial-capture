using System;
using System.Collections.Generic;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using System.Linq;
#endif

namespace GetSocialSdk.Core
{
    public class Notification : IConvertableFromNative<Notification>
    {
        /// <summary>
        /// Enumeration that allows you to have convenient switch for your action.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Custom action.
            /// </summary>
            Custom = 0,
            
            /// <summary>
            /// Profile with provided identifier should be opened.
            /// </summary>
            OpenProfile = 1,
            
            /// <summary>
            /// Activity with provided identifier should be opened.
            /// </summary>
            OpenActivity = 2,
            
            /// <summary>
            /// Open Smart Invites action.
            /// </summary>
            OpenInvites = 3,
            
            /// <summary>
            /// Open URL.
            /// </summary>
            OpenUrl = 4,
        }

        public enum NotificationTypes
        {
            
            /// <summary>
            /// Someone commented on your activity.
            /// <summary>
            Comment = 0,
    
            /// <summary>
            /// Someone liked your activity.
            /// <summary>
            LikeActivity = 1,
    
            /// <summary>
            /// Someone liked your comment.
            /// <summary>
            LikeComment = 2,
    
            /// <summary>
            /// Someone commented on the activity where you've commented before.
            /// <summary>
            CommentedInSameThread = 5,
    
            /// <summary>
            /// You became friends with another user.
            /// <summary>
            NewFriendship = 6,
    
            /// <summary>
            /// Someone accepted your invite.
            /// <summary>
            InviteAccepted = 7,
    
            /// <summary>
            /// Someone mentioned you in comment.
            /// <summary>
            MentionInComment = 8,
    
            /// <summary>
            /// Someone mentioned you in activity.
            /// <summary>
            MentionInActivity = 9,
    
            /// <summary>
            /// Someone replied to your comment.
            /// <summary>
            ReplyToComment = 10,
            //endregion
    
            /// <summary>
            /// Smart targeting Push Notifications.
            /// <summary>
            Targeting = 11,
    
            /// <summary>
            /// Notifications sent from the Dashboard when using "Test Push Notifications".
            /// <summary>
            Direct = 12
        }

        /// <summary>
        /// Contains all predefined keys for <see cref="ActionData"/> dictionary.
        /// </summary>
        public static class Key
        {
            public static class OpenActivity
            {
                public const string ActivityId = "$activity_id";
                public const string CommentId = "$comment_id";
            }

            public static class OpenProfile
            {
                public const string UserId = "$user_id";
            }
        }
        
        public string Id { get; private set; }
        public Type Action { get; private set; }
        public bool WasRead { get; private set; }
        public NotificationTypes NotificationType { get; private set; }
        public long CreatedAt { get; private set; }
        public string Title { get; private set; }
        public string Text { get; private set; }
        public Dictionary<string, string> ActionData { get; private set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Action: {1}, WasRead: {2}, NotificationType: {3}, CreatedAt: {4}, Title: {5}, Text: {6}, ActionData: {7}" 
                , Id, Action, WasRead, NotificationType, CreatedAt, Title, Text, ActionData.ToDebugString());
        }
#if UNITY_ANDROID
        public Notification ParseFromAJO(AndroidJavaObject ajo)
        {
            Id = ajo.CallStr("getId");
            Action = (Type) ajo.CallInt("getActionType");
            WasRead = ajo.CallBool("wasRead");
            NotificationType = (NotificationTypes) ajo.CallInt("getType");
            CreatedAt = ajo.CallLong("getCreatedAt");
            Title = ajo.CallStr("getTitle");
            Text = ajo.CallStr("getText");
            ActionData = ajo.CallAJO("getActionData").FromJavaHashMap();
            return this;
        }

#elif UNITY_IOS
        public Notification ParseFromJson(Dictionary<string, object> dictionary)
        {
            Title = dictionary["Title"] as string;
            Id = dictionary["Id"] as string;
            NotificationType = (NotificationTypes) (long) dictionary["Type"];
            WasRead = (bool) dictionary["WasRead"];
            CreatedAt = (long) dictionary["CreatedAt"];
            Text = dictionary["Text"] as string;
            ActionData = (dictionary["Data"] as Dictionary<string, object>).ToStrStrDict();
            Action = (Type) (long) dictionary["ActionType"];
            return this;
        }
#endif

        
    }
}