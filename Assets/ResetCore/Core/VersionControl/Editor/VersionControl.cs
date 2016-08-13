using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using ResetCore.Util;
using System.IO;
using System;

namespace ResetCore.VersionControl
{
    public class VersionControl
    {

        [InitializeOnLoad]
        public class ResetCoreLoad
        {
            static ResetCoreLoad()
            {
                bool inited = EditorPrefs.GetBool("ResetCoreInited", false);

                if (inited == false)
                {
                    foreach (VERSION_SYMBOL symbol in VersionConst.defaultSymbol)
                    {
                        AddModule(symbol);
                    }
                }

                CheckAllSymbol();
            }
        }

        public static void CheckAllSymbol()
        {
            Array symbolArr = Enum.GetValues(typeof(VERSION_SYMBOL));
            List<string> symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).ParseList(';');
            bool needRestart = false;

            for (int i = 0; i < symbolArr.Length; i++)
            {
                VERSION_SYMBOL symbol = (VERSION_SYMBOL)symbolArr.GetValue(i);
                EditorUtility.DisplayProgressBar("Check Modules", "Checking Modules " + symbol.ToString() + " " + i + "/" + symbolArr.Length, (float)i / (float)symbolArr.Length);
                //检查模块备份
                if (!Directory.Exists(VersionConst.GetSymbolTempPath(symbol)))
                {
                    if (!Directory.Exists(VersionConst.GetSymbolPath(symbol)))
                    {
                        Debug.logger.LogError("ResetCoreError", "Lose the module" + symbol.ToString());
                    }
                    else
                    {
                        PathEx.MakeDirectoryExist(VersionConst.GetSymbolTempPath(symbol));
                        DirectoryEx.DirectoryCopy(VersionConst.GetSymbolPath(symbol), VersionConst.GetSymbolTempPath(symbol), true);
                        EditorUtility.DisplayProgressBar("Check Modules", "Copy Module " + symbol.ToString() + "to backup " + i + "/" + symbolArr.Length, (float)i / (float)symbolArr.Length);
                    }
                }
                //存在宏定义 但是不存在实际模块
                if (symbols.Contains(VersionConst.SymbolName[symbol]) && !Directory.Exists(VersionConst.GetSymbolPath(symbol)))
                {
                    AddModule(symbol);
                    EditorUtility.DisplayProgressBar("Check Modules", "Add Module " + symbol.ToString() + "to ResetCore " + i + "/" + symbolArr.Length, (float)i / (float)symbolArr.Length);
                    needRestart = true;
                }
                //不存在宏定义 但是存在实际模块 移除模块
                if (!symbols.Contains(VersionConst.SymbolName[symbol]) && Directory.Exists(VersionConst.GetSymbolPath(symbol)))
                {
                    RemoveModule(symbol);
                    EditorUtility.DisplayProgressBar("Check Modules", "Remove Module " + symbol.ToString() + "from ResetCore " + i + "/" + symbolArr.Length, (float)i / (float)symbolArr.Length);
                    needRestart = true;
                }
            }
            EditorUtility.ClearProgressBar();
            if (needRestart)
            {
                EditorApplication.OpenProject(PathConfig.projectPath);
            }
        }

        //检查是否存在该模块
        public static bool ContainSymbol(VERSION_SYMBOL symbol)
        {
            List<string> symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).ParseList(';');
            return symbols.Contains(VersionConst.SymbolName[symbol]);
        }

        //添加预编译宏
        public static void AddSymbol(VERSION_SYMBOL symbol)
        {
            string symbolName = VersionConst.SymbolName[symbol];
            List<string> symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).ParseList(';');
            if (symbols.Contains(symbolName)) return;

            symbols.Add(symbolName);
            string defines = symbols.ListConvertToString(';');
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
        }

        //添加预编译宏
        public static void RemoveSymbol(VERSION_SYMBOL symbol)
        {
            string symbolName = VersionConst.SymbolName[symbol];
            List<string> symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).ParseList(';');

            if (!symbols.Contains(symbolName)) return;
            symbols.Remove(symbolName);
            string defines = symbols.ListConvertToString(';');
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);

        }

        //添加模块
        public static bool AddModule(VERSION_SYMBOL symbol)
        {

            //检查目录如果不存在则拷贝
            if (!Directory.Exists(VersionConst.GetSymbolPath(symbol)))
            {
                if (Directory.Exists(VersionConst.GetSymbolTempPath(symbol)))
                {
                    PathEx.MakeDirectoryExist(VersionConst.GetSymbolPath(symbol));
                    DirectoryEx.DirectoryCopy(VersionConst.GetSymbolTempPath(symbol), VersionConst.GetSymbolPath(symbol), true);
                }
                else
                {
                    Debug.logger.Log("can't find the module path" + VersionConst.GetSymbolPath(symbol));
                    return false;
                }
            }
            AddSymbol(symbol);

            return true;
        }

        //移除模块
        public static bool RemoveModule(VERSION_SYMBOL symbol)
        {

            //如果找不到备份文件夹则复制到备份文件夹，如果备份文件夹中有了，则直接删除
            if (!Directory.Exists(VersionConst.GetSymbolTempPath(symbol)))
            {
                PathEx.MakeDirectoryExist(VersionConst.GetSymbolTempPath(symbol));
                Debug.logger.Log("can't find the temp directory, will move the module to the temp directory");
                Directory.Move(VersionConst.GetSymbolPath(symbol), VersionConst.GetSymbolTempPath(symbol));
            }
            else
            {
                Directory.Delete(VersionConst.GetSymbolPath(symbol), true);
            }
            RemoveSymbol(symbol);

            return true;
        }

        public static void ApplySymbol(Dictionary<VERSION_SYMBOL, bool> isImportDict)
        {
            foreach (KeyValuePair<VERSION_SYMBOL, bool> isImport in isImportDict)
            {
                if (isImport.Value == false)
                {
                    RemoveSymbol(isImport.Key);
                }
                else if (isImport.Value == true)
                {
                    AddSymbol(isImport.Key);
                }
            }

            CheckAllSymbol();
        }

        public static void RefreshBackUp()
        {
            VersionControl.CheckAllSymbol();
            Directory.Delete(PathConfig.ResetCoreBackUpPath);
            VersionControl.CheckAllSymbol();
        }

    }

}
