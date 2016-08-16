using UnityEngine;
using System.Collections;
using UnityEditor;
using ResetCore.Asset;
using System;
using ResetCore.Util;
using System.Collections.Generic;
using System.IO;
using ResetCore.Data.GameDatas.Xml;

namespace ResetCore.VersionControl
{
    public class VersionControlWindow : EditorWindow
    {
        private static Dictionary<VERSION_SYMBOL, bool> isImportDict;
        private SDKManager sdkManager = new SDKManager();

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
                if (!VersionControl.isDevelopMode)
                {
                    isImportDict.Add(symbol, VersionControl.ContainSymbol(symbol));
                }
                else
                {
                    isImportDict.Add(symbol, true);
                }
            }
            inited = true;
        }

        void OnGUI()
        {
            Init();
            ShowDeveloperMode();
            ShowSymbols();
            EditorGUILayout.Space();
            ShowFunctionButton();
            ShowWhenStart();
        }

        private void ShowDeveloperMode()
        {
            GUILayout.Label("Do you want to open develop mode", GUIHelper.MakeHeader(30));
            EditorGUILayout.Space();
            VersionControl.isDevelopMode = EditorGUILayout.Toggle("Open Develop Mode", VersionControl.isDevelopMode, GUILayout.Width(200));
        }

        bool ifShowWhenStart;
        private void ShowWhenStart()
        {
            ifShowWhenStart = PlayerPrefs.GetInt("ShowResetVersionController", 1) == 1;
            ifShowWhenStart = GUILayout.Toggle(ifShowWhenStart, "Show when start");
            if (ifShowWhenStart)
            {
                PlayerPrefs.SetInt("ShowResetVersionController", 1);
            }
            else
            {
                PlayerPrefs.SetInt("ShowResetVersionController", 0);
            }
        }

        private void ShowSymbols()
        {
            GUILayout.Label("Select the module you want to use", GUIHelper.MakeHeader(30));
            EditorGUILayout.Space();

            Array versionSymbols = Enum.GetValues(typeof(VERSION_SYMBOL));
            foreach (VERSION_SYMBOL symbol in versionSymbols)
            {
                EditorGUILayout.BeginHorizontal();
                string symbolName = VersionConst.SymbolName[symbol];

                if (VersionConst.defaultSymbol.Contains(symbol))
                {
                    GUILayout.Label("Core：" + symbolName, GUIHelper.MakeHeader(30), GUILayout.Width(200));
                }
                else
                {
                    if (!VersionControl.isDevelopMode)
                    {
                        isImportDict[symbol] = EditorGUILayout.Toggle(symbolName, isImportDict[symbol], GUILayout.Width(200));
                    }
                    else
                    {
                        GUILayout.Label("Other：" + symbolName, GUIHelper.MakeHeader(30), GUILayout.Width(200));
                    }
                }
                ShowSDKSetup(symbol);

                GUILayout.Label(VersionConst.SymbolComments[symbol]);
                EditorGUILayout.EndHorizontal();
            }
        }

        private void ShowSDKSetup(VERSION_SYMBOL symbol)
        {
            if (!VersionConst.NeedSDKDict.ContainsKey(symbol) || isImportDict[symbol] == false)
                return;

            SDKType sdkType = VersionConst.NeedSDKDict[symbol];
            bool hasSetup = sdkManager.HasSetuped(sdkType);
            if (hasSetup)
            {
                GUILayout.Label("SDK：" + sdkType.ToString() +
                    " has setuped", GUIHelper.MakeHeader(30), GUILayout.Width(200));
            }
            else
            {
                if (!VersionControl.isDevelopMode)
                {
                    if (GUILayout.Button("Need Setup", GUILayout.Width(100)))
                    {
                        if (EditorUtility.DisplayDialog("Setup SDK",
                            "Do you want to setup " + sdkType.ToString() + " ?"
                            , "OK", "NO"))
                        {
                            sdkManager.SetupSDK(sdkType);

                        }
                    }
                }
                else
                {
                    GUILayout.Label("SDK：" + sdkType.ToString() +
                        " no setuped", GUIHelper.MakeHeader(30), GUILayout.Width(200));
                }
               
            }
        }

        private void ShowFunctionButton()
        {
            if (!VersionControl.isDevelopMode)
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

  
}
