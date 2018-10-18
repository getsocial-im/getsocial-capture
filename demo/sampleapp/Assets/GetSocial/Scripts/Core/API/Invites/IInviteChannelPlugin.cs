using System;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Interface for registering custom invite channel plugins such as Facebook.
    /// </summary>
    public interface InviteChannelPlugin
    {
        /// <summary>
        /// Determines whether this invite channel plugin is available for this device.
        /// </summary>
        /// <returns><c>true</c> if this invite channel plugin is available for this device; otherwise, <c>false</c>.</returns>
        /// <param name="inviteChannel">Invite channel.</param>
        bool IsAvailableForDevice(InviteChannel inviteChannel);

        /// <summary>
        /// Presents the invite channel interface.
        /// Implementation MUST guarantee that exactly one of the callbacks is eventually called. (i.e. either onComplete, onCancel or onFailure)
        /// </summary>
        /// <param name="inviteChannel">Invite channel.</param>
        /// <param name="invitePackage">Invite package containg invite being sent.</param>
        /// <param name="onComplete">On complete callback.</param>
        /// <param name="onCancel">On cancel callback.</param>
        /// <param name="onFailure">On failure callback.</param>
        void PresentChannelInterface(InviteChannel inviteChannel, InvitePackage invitePackage,
                                      Action onComplete, Action onCancel, Action<GetSocialError> onFailure);
    }
}
