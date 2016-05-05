using UnityEngine;
using System.Collections;
using UnityEditor;
using ResetCore.Asset;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace ResetCore.Util
{
    public class SyncHelper : EditorWindow
    {

        [MenuItem("Window/文件夹同步器")]
        static void ShowMainWindow()
        {
            Init();
            Rect wr = new Rect(0, 0, 800, 800);
            SyncHelper window =
                EditorWindow.GetWindowWithRect(typeof(SyncHelper), wr, true, "文件夹同步器") as SyncHelper;
            window.Show();
        }

        private static bool inited = false;

        private static readonly string DirectoryDicElName = "Directory";
        private static Dictionary<string, string> directoryDic;//From:To

        private static readonly string IgnoreFileExElName = "IngnoreEx";
        private static List<string> ignoreFileEx;

        private static List<string> alreadyExistEx;
        private static readonly string infoXmlPath = Application.dataPath + "/ResetCore/Util/Sync/Editor/SyncInfo.xml";

        private static void Init()
        {
            XDocument xDoc = new XDocument();
            if (File.Exists(infoXmlPath))
            {
                Debug.logger.Log("读取文件");
                xDoc = XDocument.Load(infoXmlPath);
                StringEx.Spriter2 = '#';
                directoryDic = StringEx.GetValue(xDoc.Root.Element(DirectoryDicElName).Value, typeof(Dictionary<string, string>)) as Dictionary<string, string>;
                //directoryDic = StringEx.ParseMap(xDoc.Root.Element(DirectoryDicElName).Value, ',', ',');
                Debug.logger.Log(xDoc.Root.Element(DirectoryDicElName).Value + "directoryDic" + directoryDic.Count);
                ignoreFileEx = StringEx.GetValue(xDoc.Root.Element(IgnoreFileExElName).Value, typeof(List<string>)) as List<string>;
                StringEx.Spriter2 = ':';
            }
            else
            {
                Debug.logger.Log("创建新文件");
                xDoc = new XDocument();
                xDoc.Add(new XElement("Root"));
                xDoc.Root.Add(new XElement(DirectoryDicElName));
                xDoc.Root.Add(new XElement(IgnoreFileExElName));
                xDoc.Save(infoXmlPath);
                AssetDatabase.Refresh();

                directoryDic = new Dictionary<string, string>();
                ignoreFileEx = new List<string>();
                alreadyExistEx = new List<string>();
            }
            FindAllEx();
            inited = true;
        }

        private static void FindAllEx()
        {
            alreadyExistEx = new List<string>();
            if (directoryDic.Count > 0)
            {
                foreach (string directoryName in directoryDic.Keys)
                {
                    if (!Directory.Exists(directoryName))
                    {
                        continue;
                    }
                    string[] fileNames = Directory.GetFiles(directoryName, "*", SearchOption.AllDirectories);
                    foreach (string fileName in fileNames)
                    {
                        //Debug.logger.Log(fileName);
                        string ex = Path.GetExtension(fileName);
                        if (!alreadyExistEx.Contains(ex))
                        {
                            alreadyExistEx.Add(ex);
                        }
                    }
                }
            }
        }

        void OnGUI()
        {
            if (inited == false)
            {
                Init();
            }
            ShowIgnoreEx();
            ShowDirectory();
            ShowSyncTools();
        }

        string ex = "扩展名";
        private void ShowIgnoreEx()
        {
            GUILayout.Label("当前忽略的扩展名有 ： " + StringEx.ConverToString(ignoreFileEx));
            GUILayout.Label("当前文件夹中拥有的的扩展名有 ： " + StringEx.ConverToString(alreadyExistEx));

            GUILayout.BeginHorizontal();
            ex = GUILayout.TextField(ex);
            if (GUILayout.Button("添加忽略扩展名", GUILayout.Width(150)))
            {
                ignoreFileEx.Add(ex);
                ignoreFileEx = ignoreFileEx.Distinct().ToList();
                SaveInfoXml();
            }
            if (GUILayout.Button("删除忽略扩展名", GUILayout.Width(150)))
            {
                ignoreFileEx.Remove(ex);
                ignoreFileEx = ignoreFileEx.Distinct().ToList();
                SaveInfoXml();
            }
            GUILayout.EndHorizontal();
        }

        string addFromPath = "";
        string addToPath = "";
        private void ShowDirectory()
        {
            GUILayout.BeginHorizontal();
            addFromPath = GUILayout.TextArea(addFromPath).Replace("\\", "/");
            addToPath = GUILayout.TextArea(addToPath).Replace("\\", "/");
            if (GUILayout.Button("添加目录", GUILayout.Width(150)))
            {
                directoryDic.Add(addFromPath, addToPath);
                FindAllEx();
                SaveInfoXml();
            }
            if (GUILayout.Button("编辑", GUILayout.Width(150)))
            {
                System.Diagnostics.Process.Start("Notepad.exe", infoXmlPath);
            }
            GUILayout.EndHorizontal();
            foreach (KeyValuePair<string, string> kvp in directoryDic)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("从" + kvp.Key + "\n同步到" + kvp.Value);
                if (GUILayout.Button("删除该目录", GUILayout.Width(150)))
                {
                    directoryDic.Remove(kvp.Key);
                    SaveInfoXml();
                }
                GUILayout.EndHorizontal();
            }
        }

        private void ShowSyncTools()
        {
            if (GUILayout.Button("同步文件夹", GUILayout.Width(150)))
            {
                foreach (KeyValuePair<string, string> kvp in directoryDic)
                {
                    string[] fileNames = Directory.GetFiles(kvp.Key, "*", SearchOption.AllDirectories);
                    foreach (string fileName in fileNames)
                    {
                        string name = fileName.Replace("\\", "/");
                        if (ignoreFileEx.Contains(Path.GetExtension(name).ToLower())) continue;

                        string from = kvp.Key.Replace("\\", "/");
                        string to = kvp.Value.Replace("\\", "/");

                        string toFilePath = name.Replace(from, to);
                        string toFileRootPath = Path.GetDirectoryName(toFilePath);

                        if (!Directory.Exists(toFileRootPath))
                        {
                            Directory.CreateDirectory(toFileRootPath);
                        }

                        File.Copy(name, toFilePath, true);
                    }
                }
                Debug.logger.Log("同步完成");
            }
            if (GUILayout.Button("刷新列表", GUILayout.Width(150)))
            {
                Init();
            }

        }

        private void SaveInfoXml()
        {
            XDocument xDoc = new XDocument();
            xDoc.Add(new XElement("Root"));
            StringEx.Spriter2 = '#';
            xDoc.Root.Add(new XElement(DirectoryDicElName, StringEx.ConverToString(directoryDic)));
            Debug.logger.Log(StringEx.ConverToString(directoryDic));
            xDoc.Root.Add(new XElement(IgnoreFileExElName, StringEx.ConverToString(ignoreFileEx)));
            StringEx.Spriter2 = ':';
            xDoc.Save(infoXmlPath);
            AssetDatabase.Refresh();
        }
    }

}
