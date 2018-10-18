using System;
using System.Collections;
using System.IO;
using System.Net;
using UnityEngine;

namespace GetSocialSdk.Editor
{
    public class DownloadFrameworkRequest
    {
     
        public bool IsInProgress { get; private set; }
        
        private WWW _downloadRequest;

        private readonly string _url;
        private readonly string _destinationFolderPath;
        private readonly string _archiveFilePath;

        public static DownloadFrameworkRequest Create(string url, string destinationFolderPath, string archiveFilePath)
        {
            return new DownloadFrameworkRequest(url, destinationFolderPath, archiveFilePath); 
        }

        private DownloadFrameworkRequest(string url, string destinationFolderPath, string archiveFilePath)
        {
            _url = url;
            _destinationFolderPath = destinationFolderPath;
            _archiveFilePath = archiveFilePath;
        }

        public void Start(Action onSuccess, Action<string> onFailure, bool sync = false)
        {
            IsInProgress = true;
            Directory.CreateDirectory(_destinationFolderPath);
            if (sync)
            {
                StartSync(onSuccess, onFailure);                
            }
            else
            {
                EditorCoroutine.Start(StartRequest(onSuccess, onFailure));
            }
        }

        public void StartSync(Action onSuccess, Action<string> onFailure)
        {
            try
            {
                using (var wc = new WebClient())
                {
                    wc.DownloadFile(new Uri(_url), _archiveFilePath);
                    onSuccess();
                    IsInProgress = false;
                }
            }
            catch (WebException)
            {
                onFailure("Can not download GetSocial frameworks, check your internet connection");
            }
            
        }

        private IEnumerator StartRequest(Action onSuccess, Action<string> onFailure)
        {
            _downloadRequest = new WWW(_url);
            while (!_downloadRequest.isDone)
            {
                yield return null;
            }
            IsInProgress = false;
            if (_downloadRequest.error != null)
            {
                onFailure("Can not download GetSocial frameworks, check your internet connection");
            }
            else
            {
                File.WriteAllBytes(_archiveFilePath, _downloadRequest.bytes);
                onSuccess();
            }
        }
    }
}