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
        
        public static bool isDevelopMode
        {
            get
            {
                return ContainSymbol(VersionConst.DeveloperSymbolName);
            }
            set
            {
                if (value == true)
                {
                    AddSymbol(VersionConst.DeveloperSymbolName);
                }
                else
                {
                    RemoveSymbol(VersionConst.DeveloperSymbolName);
                }
            }
        }



        [InitializeOnLoad]
        public class ResetCoreLoad
        {

            static ResetCoreLoad()
            {
                //检查核心数据
                CheckCoreData();

                //如果为开发者模式则关闭自动检查模块功能
                if (isDevelopMode) return;

                bool inited = EditorPrefs.GetBool("ResetCoreInited", false);

                if (inited == false)
                {
                    //将核心数据拷贝至Resources下
                    CheckCoreData();
                    //初始化模块
                    Array symbolArr = Enum.GetValues(typeof(VERSION_SYMBOL));
                    foreach (VERSION_SYMBOL symbol in VersionConst.defaultSymbol)
                    {
                        AddModule(symbol);
                    }
                    //将额外工具移至工程目录
                    MoveToolsToProject();
                    //将SDK移至工程目录备份
                    MoveSDKToTemp();

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

                EditorUtility.DisplayProgressBar("Check Modules", "Checking Modules " + symbol.ToString() + " " + (i+1) + "/" + symbolArr.Length, (float)i+1 / (float)symbolArr.Length);
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
                            symbol.ToString() + "to backup " + (i + 1) + "/" +
                            symbolArr.Length, (float)(i + 1) / (float)symbolArr.Length);
                    }
                }
                //存在宏定义 但是不存在实际模块
                if (symbols.Contains(VersionConst.SymbolName[symbol]) 
                    && (!Directory.Exists(modulePath)
                    || Directory.GetFiles(modulePath).Length == 0))
                {
                    AddModule(symbol);
                    EditorUtility.DisplayProgressBar("Check Modules", "Add Module " + 
                        symbol.ToString() + "to ResetCore " + (i + 1) + "/" + 
                        symbolArr.Length, (float)(i + 1) / (float)symbolArr.Length);
                    needRestart = true;
                }
                //不存在宏定义 但是存在实际模块 移除模块
                if (!symbols.Contains(VersionConst.SymbolName[symbol]) 
                    && Directory.Exists(modulePath)
                    && Directory.GetFiles(modulePath).Length != 0)
                {
                    RemoveModule(symbol);
                    EditorUtility.DisplayProgressBar("Check Modules", "Remove Module " + 
                        symbol.ToString() + "from ResetCore " + (i + 1) + "/" + 
                        symbolArr.Length, (float)(i + 1) / (float)symbolArr.Length);
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
            return ContainSymbol(VersionConst.SymbolName[symbol]);
        }
        public static bool ContainSymbol(string symbol)
        {
            List<string> symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup).ParseList(';');
            return symbols.Contains(symbol);
        }

        //添加预编译宏
        public static void AddSymbol(VERSION_SYMBOL symbol)
        {
            string symbolName = VersionConst.SymbolName[symbol];
            AddSymbol(symbolName);
        }
        public static void AddSymbol(string symbolName)
        {
            List<string> symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup).ParseList(';');
            if (symbols.Contains(symbolName)) return;

            symbols.Add(symbolName);
            string defines = symbols.ListConvertToString(';');
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
        }


        //移除预编译宏
        public static void RemoveSymbol(VERSION_SYMBOL symbol)
        {
            string symbolName = VersionConst.SymbolName[symbol];
            RemoveSymbol(symbolName);
        }

        public static void RemoveSymbol(string symbolName)
        {
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

        //刷新备份文件夹
        public static void RefreshBackUp()
        {
            VersionControl.CheckAllSymbol();
            Directory.Delete(PathConfig.ResetCoreBackUpPath, true);
            VersionControl.CheckAllSymbol();
        }

        
        //删除ResetCore
        public static void RemoveResetCore()
        {
            if (EditorUtility.DisplayDialog("Remove ResetCore",
                    "Do you want to remove the ResetCore? it can't be undo.", "Ok", "No"))
            {
                EditorUtility.DisplayProgressBar("Removing ResetCore", "Remove Main Finder", 0f);

                if(Directory.Exists(PathConfig.ResetCorePath))
                    Directory.Delete(PathConfig.ResetCorePath, true);

                EditorUtility.DisplayProgressBar("Removing ResetCore", "Remove Temp Finder", 0.5f);

                if (Directory.Exists(PathConfig.ResetCoreTempPath))
                    Directory.Delete(PathConfig.ResetCoreTempPath, true);

                EditorUtility.DisplayProgressBar("Removing ResetCore", "Remove SDK Finder", 0.7f);

                //移除额外工具
                if (Directory.Exists(PathConfig.SDKPath))
                    Directory.Delete(PathConfig.SDKPath, true);

                EditorUtility.DisplayProgressBar("Removing ResetCore", "Remove Symbols", 0.9f);

                //移除所有预编译
                Array symbolArr = Enum.GetValues(typeof(VERSION_SYMBOL));
                foreach(VERSION_SYMBOL symbol in symbolArr)
                {
                    RemoveSymbol(symbol);
                }
                EditorUtility.DisplayProgressBar("Removing ResetCore", "Finish", 1f);

                if(EditorUtility.DisplayDialog("Need Restart Project",
                    "You may need to Restart the project to apply your setting", "Ok", "No"))
                {
                    EditorApplication.OpenProject(PathConfig.projectPath);

                }
            }
        }

        //将核心数据拷贝至Resources下
        private static void CheckCoreData()
        {
            if (!Directory.Exists(PathConfig.localCoreDataPath) && 
                Directory.GetFiles(PathConfig.localCoreDataBackupPath).Length > 0)
            {
                if (Directory.Exists(PathConfig.localCoreDataBackupPath))
                {
                    PathEx.MakeDirectoryExist(PathConfig.localCoreDataPath);
                    DirectoryEx.DirectoryCopy(PathConfig.localCoreDataBackupPath, 
                        PathConfig.localCoreDataPath, true);
                }
                else
                {
                    Debug.logger.LogError("Core Data Error", "You Lost Your Core Data");
                }
            }
        }

        //将额外工具解压到工程目录下
        private static void MoveToolsToProject()
        {
            if (File.Exists(PathConfig.ExtraToolPathInPackage))
            {
                PathEx.MakeDirectoryExist(PathConfig.ExtraToolPath);
                CompressHelper.DecompressToDirectory(PathConfig.ExtraToolPath, PathConfig.ExtraToolPathInPackage);
            }
        }

        //将额外工具解压到工程目录下
        private static void MoveSDKToTemp()
        {
            if (File.Exists(PathConfig.SDKPathInPackage))
            {
                PathEx.MakeDirectoryExist(PathConfig.SDKBackupPath);
                CompressHelper.DecompressToDirectory(PathConfig.SDKBackupPath, PathConfig.SDKPathInPackage);
            }
        }


    }

}
