using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using ResetCore.Util;
using System.IO;
using System;
using System.Reflection;

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
                    Array symbolArr = Enum.GetValues(typeof(VERSION_SYMBOL));
                    foreach (VERSION_SYMBOL symbol in VersionConst.defaultSymbol)
                    {
                        AddModule(symbol);
                        //将额外工具移至工程目录
                        MoveToolsToProject();
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
                string tempPath = VersionConst.GetSymbolTempPath(symbol);
                string modulePath = VersionConst.GetSymbolPath(symbol);

                EditorUtility.DisplayProgressBar("Check Modules", "Checking Modules " + symbol.ToString() + " " + i + "/" + symbolArr.Length, (float)i / (float)symbolArr.Length);
                //检查模块备份
                if (!Directory.Exists(tempPath))
                {
                    if (!Directory.Exists(modulePath))
                    {
                        Debug.logger.LogError("ResetCoreError", "Lose the module " + symbol.ToString());
                    }
                    else
                    {
                        PathEx.MakeDirectoryExist(tempPath);
                        DirectoryEx.DirectoryCopy(modulePath, tempPath, true);
                        EditorUtility.DisplayProgressBar("Check Modules", "Copy Module " + 
                            symbol.ToString() + "to backup " + i + "/" + 
                            symbolArr.Length, (float)i / (float)symbolArr.Length);
                    }
                }
                //存在宏定义 但是不存在实际模块
                if (symbols.Contains(VersionConst.SymbolName[symbol]) 
                    && (!Directory.Exists(modulePath)
                    || Directory.GetFiles(modulePath).Length == 0))
                {
                    AddModule(symbol);
                    EditorUtility.DisplayProgressBar("Check Modules", "Add Module " + 
                        symbol.ToString() + "to ResetCore " + i + "/" + 
                        symbolArr.Length, (float)i / (float)symbolArr.Length);
                    needRestart = true;
                }
                //不存在宏定义 但是存在实际模块 移除模块
                if (!symbols.Contains(VersionConst.SymbolName[symbol]) 
                    && Directory.Exists(modulePath)
                    && Directory.GetFiles(modulePath).Length != 0)
                {
                    RemoveModule(symbol);
                    EditorUtility.DisplayProgressBar("Check Modules", "Remove Module " + 
                        symbol.ToString() + "from ResetCore " + i + "/" + 
                        symbolArr.Length, (float)i / (float)symbolArr.Length);
                    needRestart = true;
                }

            }
            AssetDatabase.Refresh();

            EditorUtility.ClearProgressBar();

            if (needRestart)
            {
                EditorUtility.DisplayDialog("Need Restart Project",
                    "You may need to Restart the project to apply your setting", "Ok");
            }
        }

        //检查是否存在该模块
        public static bool ContainSymbol(VERSION_SYMBOL symbol)
        {
            List<string> symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup).ParseList(';');
            return symbols.Contains(VersionConst.SymbolName[symbol]);
        }

        //添加预编译宏
        public static void AddSymbol(VERSION_SYMBOL symbol)
        {
            string symbolName = VersionConst.SymbolName[symbol];
            List<string> symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup).ParseList(';');
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
            string tempPath = VersionConst.GetSymbolTempPath(symbol);
            string modulePath = VersionConst.GetSymbolPath(symbol);

            //检查目录如果不存在则拷贝
            if (!Directory.Exists(modulePath))
            {
                if (Directory.Exists(tempPath))
                {
                    PathEx.MakeDirectoryExist(modulePath);
                    DirectoryEx.DirectoryCopy(tempPath, modulePath, true);
                }
                else
                {
                    Debug.logger.Log("can't find the module path" + modulePath);
                    return false;
                }
            }
            AddSymbol(symbol);

            return true;
        }

        //移除模块
        public static bool RemoveModule(VERSION_SYMBOL symbol)
        {
            string tempPath = VersionConst.GetSymbolTempPath(symbol);
            string modulePath = VersionConst.GetSymbolPath(symbol);

            //如果找不到备份文件夹则复制到备份文件夹，如果备份文件夹中有了，则直接删除
            if (!Directory.Exists(tempPath))
            {
                PathEx.MakeDirectoryExist(tempPath);
                Debug.logger.Log("can't find the temp directory, will move the module to the temp directory");
                if (Directory.Exists(modulePath))
                {
                    Directory.Move(modulePath, tempPath);
                    Directory.Delete(modulePath, true);
                }
            }
            else
            {
                Directory.Delete(modulePath, true);
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
            Directory.Delete(PathConfig.ResetCoreBackUpPath, true);
            VersionControl.CheckAllSymbol();
        }

        

        public static void RemoveResetCore()
        {
            if (EditorUtility.DisplayDialog("Remove ResetCore",
                    "Do you want to remove the ResetCore? it can't be undo.", "Ok", "No"))
            {
                if(Directory.Exists(PathConfig.ResetCorePath))
                    Directory.Delete(PathConfig.ResetCorePath, true);

                if (Directory.Exists(PathConfig.ResetCoreBackUpPath))
                    Directory.Delete(PathConfig.ResetCoreBackUpPath, true);

                DeleteTools();

                EditorApplication.OpenProject(PathConfig.projectPath);
            }
        }

        private static void MoveToolsToProject()
        {
            if (Directory.Exists(PathConfig.ExtraToolPathInPackage))
            {
                PathEx.MakeDirectoryExist(PathConfig.ExtraToolPath);
                Directory.Move(PathConfig.ExtraToolPathInPackage, PathConfig.ExtraToolPath);
            }
        }

        private static void DeleteTools()
        {
            if (Directory.Exists(PathConfig.ExtraToolPath))
                Directory.Delete(PathConfig.ExtraToolPath, true);
        }

    }

}
