using UnityEngine;
using System.Collections;
using UnityEditor;
using ResetCore.Asset;
using System;
using ResetCore.Util;
using System.Collections.Generic;
using System.IO;

namespace ResetCore.VersionControl
{
    public class VersionControlWindow : EditorWindow
    {
        private static Dictionary<VERSION_SYMBOL, bool> isImportDict;

        private static bool inited = false;

        [MenuItem("Tools/ResetCore Module Controller")]
        static void ShowMainWindow()
        {
            VersionControlWindow window =
                EditorWindow.GetWindow(typeof(VersionControlWindow), false, "Module Controller") as VersionControlWindow;
            window.Show();
        }

        private static void Init()
        {
            if (inited) return;
            isImportDict = new Dictionary<VERSION_SYMBOL, bool>();
            Array versionSymbols = Enum.GetValues(typeof(VERSION_SYMBOL));
            foreach (VERSION_SYMBOL symbol in versionSymbols)
            {
                isImportDict.Add(symbol, VersionControl.ContainSymbol(symbol));
            }
            inited = true;
        }

        void OnGUI()
        {
            Init();
            ShowSymbols();
            EditorGUILayout.Space();
            ShowFunctionButton();
        }

        private void ShowSymbols()
        {
            GUILayout.Label("Select the module you want to use", GUIHelper.MakeHeader(30));
            EditorGUILayout.Space();

            Array versionSymbols = Enum.GetValues(typeof(VERSION_SYMBOL));
            foreach (VERSION_SYMBOL symbol in versionSymbols)
            {
                EditorGUILayout.BeginHorizontal();
                if (VersionConst.defaultSymbol.Contains(symbol))
                {
                    GUILayout.Label("核心：" + VersionConst.SymbolName[symbol], GUIHelper.MakeHeader(30), GUILayout.Width(200));
                }
                else
                {
                    isImportDict[symbol] = EditorGUILayout.Toggle(VersionConst.SymbolName[symbol], isImportDict[symbol], GUILayout.Width(200));
                }
                GUILayout.Label(VersionConst.SymbolComments[symbol]);
                EditorGUILayout.EndHorizontal();
            }
        }

        private void ShowTitles()
        {
            
        }

        private void ShowFunctionButton()
        {
            if (GUILayout.Button("Apply", GUILayout.Width(200)))
            {
                VersionControl.ApplySymbol(isImportDict);
                inited = false;
            }
            if (GUILayout.Button("Refresh Backup", GUILayout.Width(200)))
            {
                VersionControl.RefreshBackUp();
                inited = false;
            }
            if (GUILayout.Button("Remove ResetCore", GUILayout.Width(200)))
            {
                VersionControl.RemoveResetCore();
                inited = false;
            }
        }

       
    }

  
}
