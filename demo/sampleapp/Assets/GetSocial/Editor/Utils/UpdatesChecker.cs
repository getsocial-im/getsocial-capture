/**
*     Copyright 2015-2016 GetSocial B.V.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Net;
using GetSocialSdk.Core;
using GetSocialSdk.MiniJSON;
using UnityEngine;

namespace GetSocialSdk.Editor
{
    [InitializeOnLoad]
    public static class UpdatesChecker
    {
        enum NewVersionCheckSource
        {
            OnLoad,
            OnUserCheckedForUpdate
        }

        // Check for updates
        const string NewVersionAvailableFormat = "GetSocial plugin v{0} is now available!";

        static UpdatesChecker()
        {
            CheckForUpdatesOnReleaseRepo(NewVersionCheckSource.OnLoad);
        }

        [MenuItem("GetSocial/Check for Updates...", false, 2000)]
        public static void CheckForUpdates()
        {
            CheckForUpdatesOnReleaseRepo(NewVersionCheckSource.OnUserCheckedForUpdate);
        }

        private static string LatestReleaseURL()
        {
            const string latestReleaseApiUrl = "https://s3.amazonaws.com/downloads.getsocial.im/unity/releases/latest.json";
            const string latestReleaseAssetStoreApiUrl = "https://s3.amazonaws.com/downloads.getsocial.im/unity/releases/latest-assets-store.json";

            return AssetStoreUtils.IsAssetStorePackage() 
                ? latestReleaseAssetStoreApiUrl 
                : latestReleaseApiUrl;
        }

        static Dictionary<string, object> GetLastReleaseInfo()
        {
            var request = WebRequest.Create(LatestReleaseURL()) as HttpWebRequest;

            request.Method = "GET";
            request.UserAgent = "Unity Editor";
            request.Proxy = null;
            request.KeepAlive = false;
            request.ContentType = "application/json";

            try
            {
                using (var response = request.GetResponse() as HttpWebResponse)
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return GSJson.Deserialize(reader.ReadToEnd()) as Dictionary<string, object>;
                }
            }
            catch (Exception)
            {
                return new Dictionary<string, object>();
            }
        }

        static void CheckForUpdatesOnReleaseRepo(NewVersionCheckSource source)
        {
            var latestReleaseInfo = GetLastReleaseInfo();

            if (latestReleaseInfo.Count != 0)
            {
                var version = (string) latestReleaseInfo["version"];
                var downloadUrl = (string) latestReleaseInfo["url"];

                if (source == NewVersionCheckSource.OnLoad)
                {
                    LogNewVersionAvailable(version);
                }
                else
                {
                    ShowUpdateDialog(version, downloadUrl);
                }
            }
        }

        static void LogNewVersionAvailable(string latestReleaseTag)
        {
            if (IsNewSdkVersionAvailable(latestReleaseTag))
            {
                Debug.Log(string.Format(NewVersionAvailableFormat, latestReleaseTag) + ". Go to GetSocial -> Check for Updates to download the latest version");
            }
        }

        static void ShowUpdateDialog(string latestReleaseVersion, string downloadUrl)
        {
            var dialogTitle = "Update GetSocial SDK";
            if (IsNewSdkVersionAvailable(latestReleaseVersion))
            {
                var downloadUpdate = EditorUtility.DisplayDialog(dialogTitle,
                    string.Format(NewVersionAvailableFormat, latestReleaseVersion), "Download Update", "Cancel");
                if (downloadUpdate)
                {
                    Application.OpenURL(downloadUrl);
                }
            }
            else
            {
                EditorUtility.DisplayDialog(dialogTitle, "You already have the latest version installed (" + BuildConfig.UnitySdkVersion + ")", "OK");
            }
        }

        static bool IsNewSdkVersionAvailable(string latestVersion)
        {
            return SemanticVersion.Parse(latestVersion) > SemanticVersion.Parse(BuildConfig.UnitySdkVersion);
        }
    }
}