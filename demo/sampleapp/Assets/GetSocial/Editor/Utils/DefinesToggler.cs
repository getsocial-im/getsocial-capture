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
using System.Linq;

namespace GetSocialSdk.Editor
{
    public static class DefinesToggler
    {
        const char DefineSymbolsSeparator = ';';
        static readonly string DefineSymbolsSeparatorStr = ';'.ToString();
        const string EnableGetSocialUiDefineSymbol = "USE_GETSOCIAL_UI";

        static readonly BuildTargetGroup[] GetSocialTargets =
        {
            BuildTargetGroup.Android,
            BuildTargetGroup.iOS,
            BuildTargetGroup.Standalone
        };

        #region USE_GETSOCIAL_UI

        public static void ToggleUseGetSocialUiDefine(bool enable)
        {
            try
            {
                if (enable)
                {
                    AddDefineToAll(EnableGetSocialUiDefineSymbol, GetSocialTargets);
                }
                else
                {
                    RemoveDefineFromAll(EnableGetSocialUiDefineSymbol, GetSocialTargets);
                }
            }
            catch (Exception)
            {

            }
        }

        public static bool IsUseGetSocialUiDefinePresent()
        {
            bool isPresent = true;

            foreach (var targetGroup in GetSocialTargets)
            {
                var defineSymbolsString = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
                var defineSymbols = new List<string>(defineSymbolsString.Split(DefineSymbolsSeparator));

                var containsEnabledUiSymbol = defineSymbols.Contains(EnableGetSocialUiDefineSymbol);
                isPresent &= containsEnabledUiSymbol;
            }

            return isPresent;
        }
        #endregion

        #region generic
        public static void ToggleDefine(string define, bool enable, params BuildTargetGroup[] supportedPlatforms)
        {
            foreach (var targetPlatform in supportedPlatforms)
            {
                ToggleDefine(define, enable, targetPlatform);
            }
        }

        public static void ToggleDefine(string define, bool enable, BuildTargetGroup targetPlatform)
        {
            var scriptDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetPlatform);
            var flags = new List<string>(scriptDefines.Split(DefineSymbolsSeparator));

            if (flags.Contains(define))
            {
                if (!enable)
                {
                    flags.Remove(define);
                }
            }
            else
            {
                if (enable)
                {
                    flags.Add(define);
                }
            }

            var result = string.Join(DefineSymbolsSeparatorStr, flags.ToArray());

            if (scriptDefines != result)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetPlatform, result);
            }
        }

        public static void RemoveDefineFromAll(string define, BuildTargetGroup[] platforms)
        {
            ToggleDefine(define, false, platforms);
        }

        public static void AddDefineToAll(string define, BuildTargetGroup[] platforms)
        {
            ToggleDefine(define, true, platforms);
        }

        static IEnumerable<BuildTargetGroup> GetAllAvailablePlatforms()
        {
            var allPlatforms = Enum.GetValues(typeof (BuildTargetGroup)).Cast<BuildTargetGroup>().ToList();
            allPlatforms.Remove(BuildTargetGroup.Unknown);
            return allPlatforms.ToArray();
        }
        #endregion
    }
}