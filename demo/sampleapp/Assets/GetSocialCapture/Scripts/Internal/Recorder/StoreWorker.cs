using System.Threading;
using GetSocialSdk.Capture.Scripts.Internal.Gif;
using GetSocialSdk.Scripts.Internal.Util;
using UnityEngine;
using ThreadPriority = System.Threading.ThreadPriority;

namespace GetSocialSdk.Capture.Scripts.Internal.Recorder
{
    /// <summary>
    /// Stores captured frames in memory in FixedSizedQueue
    /// </summary>
    public sealed class StoreWorker
    {

        #region Public fields

        /// <summary>
        /// Queue storing captured frames.
        /// </summary>
        public FixedSizedQueue<GifFrame> StoredFrames { get; private set; }
        
        #endregion

        #region Internal fields

        internal static StoreWorker Instance
        {
            get { return _instance?? (_instance = new StoreWorker()); }
        }

        #endregion
        
        #region Private fields

        private Thread _thread;
        private static StoreWorker _instance;

        #endregion

        #region Internal methods

        internal void Start(ThreadPriority priority, int maxCapturedFrames)
        {
            StoredFrames = new FixedSizedQueue<GifFrame>(maxCapturedFrames);
            _thread = new Thread(Run) {Priority = priority};
            _thread.Start();
        }

        internal void StoreFrame(RenderTexture renderTexture)
        {
            // convert to Texture2D
            var tempTexture =
                new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false)
                {
                    hideFlags = HideFlags.HideAndDontSave,
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = FilterMode.Bilinear,
                    anisoLevel = 0
                };

            RenderTexture.active = renderTexture;
            tempTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            RenderTexture.active = null;
            
            var frame = new GifFrame
            {
                Width = tempTexture.width,
                Height = tempTexture.height,
                Data = tempTexture.GetPixels32()
            };

            Object.Destroy(tempTexture);
            
            StoredFrames.Enqueue(frame);
        }

        #endregion

        #region Private methods

        private static void Run()
        {
            
        }

        #endregion

    }
}