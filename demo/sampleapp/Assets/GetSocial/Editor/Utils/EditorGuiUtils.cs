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
using UnityEngine;

namespace GetSocialSdk.Editor
{
    public static class EditorGuiUtils
    {
        public static void SelectableLabelField(GUIContent label, string value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Height(16), OneThirdWidth);
            EditorGUILayout.SelectableLabel(value, GUILayout.Height(16), TwoThirdsWidth);
            EditorGUILayout.EndHorizontal();
        }

        public static GUIContent GetBoldLabel(string text, string tooltip = "", Texture2D icon = null)
        {
            var label = icon != null ? new GUIContent(text, icon, tooltip) : new GUIContent(text, tooltip);
            var style = EditorStyles.foldout;
            style.fontStyle = FontStyle.Bold;
            return label;
        }

        public static GUILayoutOption OneThirdWidth
        {
            get { return GUILayout.Width((EditorGUIUtility.currentViewWidth - 60) / 3); }
        }
        
        public static GUILayoutOption TwoThirdsWidth
        {
            get { return GUILayout.Width((EditorGUIUtility.currentViewWidth - 60) / 3 * 2); }
        }
        
        public static GUILayoutOption HalfWidth
        {
            get { return GUILayout.Width((EditorGUIUtility.currentViewWidth - 60) / 2); }
        }

        public static void ColoredBackground(Color backgroundColor, Action drawAction)
        {
            var backupColor = GUI.backgroundColor;
            GUI.backgroundColor = backgroundColor;

            drawAction();

            GUI.backgroundColor = backupColor;
        }
    }
    
    //FixedWidthLabel class. Extends IDisposable, so that it can be used with the "using" keyword.
    public class FixedWidthLabel : IDisposable
    {
        private const int Indent = 9;
        private readonly ZeroIndent _indentReset;

        public FixedWidthLabel(GUIContent label)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, 
                GUILayout.Width(GUI.skin.label.CalcSize(label).x 
                                + Indent * EditorGUI.indentLevel));

            _indentReset = new ZeroIndent();
        }

        public FixedWidthLabel(string label) : this(new GUIContent(label))
        {
        }

        public void Dispose()
        {
            _indentReset.Dispose();
            EditorGUILayout.EndHorizontal();
        }
    }

    // Helper class to clear indentation
    class ZeroIndent : IDisposable 
    {
        private readonly int _originalIndent;
        public ZeroIndent()
        {
            _originalIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
        }

        public void Dispose()
        {
            EditorGUI.indentLevel = _originalIndent;
        }
    }
}
