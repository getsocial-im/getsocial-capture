using System;
using System.Threading;
using GetSocialSdk.Capture.Scripts.Internal.Gif;
using GetSocialSdk.Scripts.Internal.Util;
using UnityEngine;
using Object = UnityEngine.Object;
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

        internal void Clear()
        {
            if (StoredFrames != null)
            {
                StoredFrames.Clear();
                StoredFrames = null;
            }
            if (_thread != null)
            {
                _thread.Abort();
                _thread = null;
            }
        }

        internal void Start(ThreadPriority priority, int maxCapturedFrames)
        {
            // make sure everything is cleared from previous session
            Clear();
            StoredFrames = new FixedSizedQueue<GifFrame>(maxCapturedFrames);
            _thread = new Thread(Run) {Priority = priority};
            _thread.Start();
        }

        internal void StoreFrame(RenderTexture renderTexture, double resizeRatio)
        {
            var newWidth = Convert.ToInt32(renderTexture.width * resizeRatio);
            var newHeight = Convert.ToInt32(renderTexture.height * resizeRatio);
            renderTexture.filterMode = FilterMode.Bilinear;
            
            var resizedRenderTexture = RenderTexture.GetTemporary(newWidth, newHeight);
            resizedRenderTexture.filterMode = FilterMode.Bilinear;

            RenderTexture.active = resizedRenderTexture;
            Graphics.Blit(renderTexture, resizedRenderTexture);
            
            // convert to Texture2D
            var resizedTexture2D =
                new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, false)
                {
                    hideFlags = HideFlags.HideAndDontSave,
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = FilterMode.Bilinear,
                    anisoLevel = 0
                };

            resizedTexture2D.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
            resizedTexture2D.Apply();            
            RenderTexture.active = null;

            var frame = new GifFrame
            {
                Width = resizedTexture2D.width,
                Height = resizedTexture2D.height,
                Data = resizedTexture2D.GetPixels32()
            };

            resizedRenderTexture.Release();
            Object.Destroy(resizedTexture2D);
            
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