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
using UnityEditor.iOS.Xcode.GetSocial;
using UnityEngine;


namespace GetSocialSdk.Editor
{
    public static class PBXProjectUtils
    {
        public static void ModifyPbxProject(string projectPath, Action<PBXProject, string> projectModifier,
            string errorMsg = "One of GetSocial postprocessing stages failed. See the error above.")
        {
            try
            {
                var pbxprojPath = PBXProject.GetPBXProjectPath(projectPath);

                var project = new PBXProject();
                project.ReadFromString(File.ReadAllText(pbxprojPath));

                var target = project.TargetGuidByName(PBXProject.GetUnityTargetName());

                projectModifier(project, target);

                File.WriteAllText(pbxprojPath, project.WriteToString());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.LogError(errorMsg);
            }
        }

        public static void ModifyPlist(string projectPath, Action<PlistDocument> plistModifier,
            string errorMsg = "Failed to update Info.plist in Xcode project.")
        {
            Debug.Log(string.Format("Updating Info.plist in Xcode project for '{0}'", projectPath));

            try
            {
                var plistInfoFile = new PlistDocument();

                var infoPlistPath = Path.Combine(projectPath, "Info.plist");
                plistInfoFile.ReadFromString(File.ReadAllText(infoPlistPath));

                plistModifier(plistInfoFile);

                File.WriteAllText(infoPlistPath, plistInfoFile.WriteToString());
                Debug.Log("Info.plist successfully updated in Xcode project.");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.LogError(errorMsg);
            }
        }

        public static bool ContainsKey(this PlistDocument plist, string key)
        {
            return plist.root.values.ContainsKey(key);
        }
    }
}