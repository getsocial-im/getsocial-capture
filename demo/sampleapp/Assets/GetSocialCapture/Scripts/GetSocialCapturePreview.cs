using System;
using System.Collections.Generic;
using GetSocialSdk.Capture.Scripts.Internal.Recorder;
using UnityEngine;
using UnityEngine.UI;

namespace GetSocialSdk.Capture.Scripts
{
    public class GetSocialCapturePreview : MonoBehaviour
    {

        #region Public fields

        /// <summary>
        /// Number of displayed frames per second. Default is 30.
        /// </summary>
        public int playbackFrameRate = 30;
        
        /// <summary>
        /// Preview loops or played only once.
        /// </summary>
        public bool loopPlayback = true;

        #endregion

        #region Private fields

        private List<Texture2D> _framesToPlay;
        private RawImage _rawImage;
        private bool _play;
        private float _playbackStartTime;
        private bool _previewInitialized;

        #endregion

        #region Public methods

        /// <summary>
        /// Starts preview playback.
        /// </summary>
        public void Play()
        {
            if (!_previewInitialized)
            {
                Init();
            }
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            _play = true;
        }

        /// <summary>
        /// Stops playback.
        /// </summary>
        public void Stop()
        {
            _play = false;
        }
        
        #endregion

        #region Private methods

        private void Init()
        {
            for (var i = 0; i < StoreWorker.Instance.StoredFrames.Count(); i ++)
            {
                var frame = StoreWorker.Instance.StoredFrames.ElementAt(i);
                var texture2D = new Texture2D(frame.Width, frame.Height);
                texture2D.SetPixels32(frame.Data);
                texture2D.Apply();
                _framesToPlay.Add(texture2D);
            }

            _previewInitialized = true;
        }

        #endregion
                
        #region Unity methods

        private void Awake()
        {
            _rawImage = GetComponent<RawImage>();
            _framesToPlay = new List<Texture2D>();
            _play = false;
        }

        private void OnDestroy()
        {
            var listId = GC.GetGeneration(_framesToPlay);
            _framesToPlay.Clear();
            GC.Collect(listId, GCCollectionMode.Forced);
            _framesToPlay = null;
        }

        private void Start()
        {
            Init();
            if (_framesToPlay.Count == 0)
            {
                _play = false;
            }
        }

        private void Update()
        {
            if (!_play) return;
            if (_framesToPlay.Count == 0) return;
            if (Math.Abs(_playbackStartTime) < 0.0001f)
            {
                _playbackStartTime = Time.time;
            }
            var index = (int) ((Time.time - _playbackStartTime) * playbackFrameRate) % _framesToPlay.Count;
            _rawImage.texture = _framesToPlay[index];
            if (index == _framesToPlay.Count - 1 && !loopPlayback)
            {
                _play = false;
            }
        }

        #endregion

    }
}