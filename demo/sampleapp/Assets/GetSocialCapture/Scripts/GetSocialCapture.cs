using System;
using System.Collections;
using System.IO;
using GetSocialSdk.Capture.Scripts.Internal.Gif;
using GetSocialSdk.Capture.Scripts.Internal.Recorder;
using GetSocialSdk.Scripts.Internal.Util;
using UnityEngine;

using ThreadPriority = System.Threading.ThreadPriority;

namespace GetSocialSdk.Capture.Scripts
{
    
    [RequireComponent(typeof(Camera)), DisallowMultipleComponent]
    public class GetSocialCapture : MonoBehaviour
    {

        /// <summary>
        /// Defines how frames are captured.
        /// </summary>
        public enum GetSocialCaptureMode
        {
            /// <summary>
            /// Frames captured continuously with the give frame rate.
            /// </summary>
            Continuous = 0,
            
            /// <summary>
            /// CaptureFrame() has to be called to make a capture.
            /// </summary>
            Manual
        }
    
        #region Public fields

        /// <summary>
        /// Number of captured frames per second. Default is 10.
        /// </summary>
        public int captureFrameRate = 10;

        /// <summary>
        /// Capture mode.
        /// </summary>
        public GetSocialCaptureMode captureMode = GetSocialCaptureMode.Continuous;

        /// <summary>
        /// Max. number of captured frames during the session. Default is 50.
        /// </summary>
        public int maxCapturedFrames = 50;

        /// <summary>
        /// Number of displayed frames per second. Default is 30.
        /// </summary>
        public int playbackFrameRate = 30;

        /// <summary>
        /// Generated gif loops or played only once.
        /// </summary>
        public bool loopPlayback = true;

        /// <summary>
        /// Captured content.
        /// </summary>
        public Camera capturedCamera;
        
        #endregion

        #region Private variables

        private string _captureId;
        private string _resultFilePath;
        private float _elapsedTime;
        private Recorder _recorder;

        private const string GeneratedContentFolderName = "gifresult";

        #endregion

        #region Public methods

        public void StartCapture()
        {
            if (_captureId != null)
            {
                CleanUp();
            }
            InitSession();
            _recorder.CurrentState = Recorder.RecordingState.Recording;
        }

        public void StopCapture()
        {
            _recorder.CurrentState = Recorder.RecordingState.OnHold;
        }

        public void ResumeCapture()
        {
            if (_captureId == null)
            {
                Debug.Log("There is no previous capture session to continue");
            }
            else
            {
                _recorder.CurrentState = Recorder.RecordingState.Recording;
            }
        }

        public void CaptureFrame()
        {
            if (_captureId == null)
            {
                InitSession();                                
            }
            _recorder.CurrentState = Recorder.RecordingState.RecordNow;
        }

        public void GenerateCapture(Action<byte[]> result)
        {
            _recorder.CurrentState = Recorder.RecordingState.OnHold;
            if (StoreWorker.Instance.StoredFrames.Count() > 0)
            {
                var generator = new GeneratorWorker(loopPlayback, playbackFrameRate, ThreadPriority.BelowNormal, StoreWorker.Instance.StoredFrames,
                     _resultFilePath,
                    () =>
                    {
                        Debug.Log("Result: " + _resultFilePath);
                        
                        MainThreadExecutor.Queue(() => {
                            result(File.ReadAllBytes(_resultFilePath));
                            
                        });
                    });
                generator.Start();
            }
            else
            {
                Debug.Log("Something went wrong, check your settings");
                result(new byte[0]);
            }
        }

        #endregion

        #region Unity methods

        private void Awake()
        {
            if (capturedCamera == null)
            {
                capturedCamera = GetComponent<Camera>();
            }

            if (capturedCamera == null)
            {
                Debug.LogError("Camera is not set");
                return;
            }
            _recorder = capturedCamera.GetComponent<Recorder>(); 
            if (_recorder == null)
            {
                _recorder = capturedCamera.gameObject.AddComponent<Recorder>();
            }

            _recorder.CaptureFrameRate = captureFrameRate;
        }

        private void OnDestroy()
        {
            StoreWorker.Instance.Clear();
        }

        #endregion

        #region Private methods

        private static string GetResultDirectory()
        {
            string resultDirPath;
            #if UNITY_EDITOR
                resultDirPath = Application.dataPath; 
            #else
                resultDirPath = Application.persistentDataPath; 
            #endif
            resultDirPath += Path.DirectorySeparatorChar + GeneratedContentFolderName;

            if (!Directory.Exists(resultDirPath))
            {
                Directory.CreateDirectory(resultDirPath);
            }
            return resultDirPath;
        }

        private void InitSession()
        {
            _captureId = Guid.NewGuid().ToString();
            var fileName = string.Format("result-{0}.gif", _captureId);
            _resultFilePath = GetResultDirectory() + Path.DirectorySeparatorChar + fileName;
            StoreWorker.Instance.Start(ThreadPriority.BelowNormal, maxCapturedFrames);
        }
        
        private void CleanUp()
        {
            if (File.Exists(_resultFilePath))
            {
                File.Delete(_resultFilePath);
            }
        }
        
        #endregion
        
    }    

}


