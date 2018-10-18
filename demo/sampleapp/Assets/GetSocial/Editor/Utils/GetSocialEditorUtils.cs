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
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GetSocialSdk.Editor
{
    [InitializeOnLoad]
    public static class GetSocialEditorUtils
    {
        public static Texture2D AndroidIcon;
        public static Texture2D IOSIcon;
        public static Texture2D SettingsIcon;
        public static Texture2D InfoIcon;
        public static Texture2D GetSocialIcon;

        static string _editorPath;
        static string _editorGuiPath;
        
        private static string signingKeyHash;
        private static string keystoreUtilError;
        private static string previousKeystorePath;
        private static string previousKeystorePass;
        private static string previousKeyAlias;

        static GetSocialEditorUtils()
        {
            Initialize();
            AndroidIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(_editorGuiPath + "/android.png", typeof(Texture2D));
            IOSIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(_editorGuiPath + "/ios.png", typeof(Texture2D));
            SettingsIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(_editorGuiPath + "/settings.png", typeof(Texture2D));
            InfoIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(_editorGuiPath + "/icon_info.png", typeof(Texture2D));
            GetSocialIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(_editorGuiPath + "/getsocial.png", typeof(Texture2D));
        }

        static void Initialize()
        {
            var rootDir = new DirectoryInfo(Application.dataPath);
            var files = rootDir.GetFiles("GetSocialSettingsEditor.cs", SearchOption.AllDirectories);
            _editorPath = Path.GetDirectoryName(files[0].FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets"));
            _editorGuiPath = Path.Combine(_editorPath, "GUI");

            CheckNativeLibraries();
        }

        public static void BeginSetSmallIconSize()
        {
            EditorGUIUtility.SetIconSize(new Vector2(14, 14));
        }

        public static void EndSetSmallIconSize()
        {
            EditorGUIUtility.SetIconSize(Vector2.zero);
        }

        public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            var searchPatterns = searchPattern.Split(';');
            var files = new List<string>();
            foreach (string sp in searchPatterns)
            {
                files.AddRange(Directory.GetFiles(path, sp, searchOption));
            }
            files.Sort();
            return files.ToArray();
        }
        
        public static string SigningKeyHash
        {
            get
            {
                string keystorePath = DebugKeyStorePath;
                string keystorePass = "android";
                string keyAlias = "androiddebugkey";

                if (UserDefinedKeystore())
                {
                    keystorePath = PlayerSettings.Android.keystoreName;
                    keystorePass = PlayerSettings.Android.keystorePass;
                    keyAlias = PlayerSettings.Android.keyaliasName;
                    if (!KeystorePassDefined())
                    {
                        keystoreUtilError = "Keystore password is not set.";
                        return "";
                    }
                }

                bool settingsAreSame = keystorePath.Equals(previousKeystorePath) &&
                                         keystorePass.Equals(previousKeystorePass) &&
                                         keyAlias.Equals(previousKeyAlias);
                
                if ((signingKeyHash == null && keystoreUtilError == null) || !settingsAreSame)
                {
                    keystoreUtilError = null;
                    previousKeystorePath = keystorePath;
                    previousKeystorePass = keystorePass;
                    previousKeyAlias = keyAlias;
                    
                    if (!HasAndroidKeystoreFile(keystorePath))
                    {
                        keystoreUtilError = "Error: Can't find Android keystore " + keystorePath;
                        return "";
                    }
                    if (!DoesKeytoolExist())
                    {
                        keystoreUtilError = "Error: keytool command line utility does not exist.";
                        return "";
                    }
                    if (!HasAndroidSdk())
                    {
                        keystoreUtilError = "Error: Can't find Android Sdk.";
                        return "";
                    }
                    signingKeyHash = GetKeyHash(keystorePath, keystorePass, keyAlias); 
                }
                return signingKeyHash;
            }
        }

        public static bool UserDefinedKeystore()
        {
            return !string.IsNullOrEmpty(PlayerSettings.Android.keystoreName);
        }

        public static bool KeystorePassDefined()
        {
            return !string.IsNullOrEmpty(PlayerSettings.Android.keystorePass);
        }

        public static string KeyStoreUtilError
        {
            get
            {
                return keystoreUtilError;
            }
        }

        private static string DebugKeyStorePath
        {
            get
            {
                return (Application.platform == RuntimePlatform.WindowsEditor) ?
                    Environment.GetEnvironmentVariable("HOMEDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH") + @"\.android\debug.keystore" :
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"/.android/debug.keystore";
            }
        }

        private static bool HasAndroidSdk()
        {
            return EditorPrefs.HasKey("AndroidSdkRoot") && Directory.Exists(EditorPrefs.GetString("AndroidSdkRoot"));
        }
        
        private static bool HasAndroidKeystoreFile(string keystorePath)
        {
            return File.Exists(keystorePath);
        }
        
        private static string GetKeyHash(string keystoreName, string keystorePassword, string aliasName)
        {
            var proc = new Process();
            string arguments;
            if (aliasName != null)
            {
                arguments = @"""keytool -list -v -keystore {0} -storepass {1} -alias {2}"""; 
            }
            else
            {
                arguments = @"""keytool -list -v -keystore {0} -storepass {1}""";
            } 
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                proc.StartInfo.FileName = "cmd";
                arguments = @"/C " + arguments;
            }
            else
            {
                proc.StartInfo.FileName = "bash";
                arguments = @"-c " + arguments;
            }

            if (aliasName != null)
            {
                proc.StartInfo.Arguments = string.Format(arguments, keystoreName, keystorePassword, aliasName);
            }
            else
            {
                proc.StartInfo.Arguments = string.Format(arguments, keystoreName, keystorePassword);
            }

            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            var keyHash = new StringBuilder();
            while (!proc.HasExited)
            {
                keyHash.Append(proc.StandardOutput.ReadToEnd());
            }

            if (proc.ExitCode == 255)
            {
                return "";
            }

            string response = keyHash.ToString();

            const string errorLiteral = "keytool error:";
            if (response.Contains(errorLiteral))
            {
                int errorBeginIndex = response.IndexOf("Exception:") + "Exception:".Length + 1;
                int errorEndIndex = response.IndexOf('\n', errorBeginIndex);
                keystoreUtilError = "Error: " + response.Substring(errorBeginIndex, (errorEndIndex - errorBeginIndex)).Trim();
                return "";
            }
            const string sha256Literal = "SHA256:";
            if (response.Contains(sha256Literal))
            {
                int shaBeginIndex = response.IndexOf(sha256Literal) + sha256Literal.Length;
                int shaEndIndex = response.IndexOf('\n', shaBeginIndex);
                return response.Substring(shaBeginIndex, (shaEndIndex - shaBeginIndex)).Trim();
            }
            keystoreUtilError = "Error: Can't read signature, Check Player Settings -> Android -> Publishing Settings";
            return "";
        }
        
        private static bool DoesKeytoolExist()
        {
            var proc = new Process();
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                proc.StartInfo.FileName = "cmd";
                proc.StartInfo.Arguments = @"/C keytool";
            }
            else
            {
                proc.StartInfo.FileName = "bash";
                proc.StartInfo.Arguments = @"-c keytool";
            }

            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            proc.WaitForExit();
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                return proc.ExitCode == 0;
            }
            else
            {
                return proc.ExitCode != 127;
            }
        }

        private static void CheckNativeLibraries()
        {
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                if (!FileHelper.CheckiOSFramework() && !FileHelper.IOSDownloadInProgress)
                {
                    FileHelper.DownloadiOSFramework();                    
                }
            }

            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                if (!FileHelper.CheckAndroidFramework() && !FileHelper.AndroidDownloadInProgress)
                {
                    FileHelper.DownloadAndroidFramework();
                }
                
            }
        }
        

    }
}
