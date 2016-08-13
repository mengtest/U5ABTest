using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Xml.Linq;
using System.IO;
using UnityEngine.SceneManagement;

namespace ResetCore.Asset
{
    [InitializeOnLoad]
    public class ResourcesListGenWhenLoad
    {
        static ResourcesListGenWhenLoad()
        {
            EditorApplication.update += Update;
        }

        static void Update()
        {
            bool isSuccess = EditorApplication.ExecuteMenuItem(ResourcesListGen.menuItemName);
            if (isSuccess) EditorApplication.update -= Update;
        }
    }

    public class ResourcesListGen
    {

        private static readonly string[] ignoreFliter = new string[] { ".meta", ".unity", ".shader" };

        public const string menuItemName = "Tools/更新资源列表";
        [MenuItem(menuItemName)]
        public static void UpdateResourcesList()
        {
            XDocument resourceListDoc = new XDocument(
                new XElement("Root")
                );
            XElement rootEl = resourceListDoc.Element("Root");

            PathEx.MakeDirectoryExist(PathConfig.resourcePath);
            DirectoryInfo resourceFolder = new DirectoryInfo(PathConfig.resourcePath);
            FileInfo[] fileInfos = resourceFolder.GetFiles("*", SearchOption.AllDirectories);

            
            int currentNum = 0;
            int total = fileInfos.Length;
            EditorUtility.DisplayProgressBar("更新本地资源列表中", "0/" + total, 0);
            foreach (FileInfo info in fileInfos)
            {
                string name = info.Name;
                string path = GetPathWithoutEx(info.FullName);

                //path = Path.GetFileNameWithoutExtension(path);

                if (IsResource(name))
                {
                    rootEl.Add(new XElement("n", name));
                    rootEl.Add(new XElement("p", path));
                }
                currentNum++;
                EditorUtility.DisplayProgressBar("更新本地资源列表中", currentNum + "/" + total + info.Name, (float)currentNum/(float)total);
            }

            EditorUtility.ClearProgressBar();

            if (!Directory.Exists(PathConfig.resourcePath + PathConfig.resourceBundlePath))
            {
                Directory.CreateDirectory(PathConfig.resourcePath + PathConfig.resourceBundlePath);
            }

            resourceListDoc.Save(PathConfig.resourcePath + PathConfig.resourceListDocPath + ".xml");
            AssetDatabase.Refresh();
            Debug.logger.Log("更新本地资源列表完成");
            
        }

        private static bool IsResource(string path)
        {
            string extension = Path.GetExtension(path);
            foreach (string ignoreEx in ignoreFliter)
            {
                if (extension == ignoreEx)
                {
                    return false;
                }
            }
            return true;
        }

        private static string GetPathWithoutEx(string path)
        {
            string filePath = path;
            DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath);
            filePath = filePath.Replace(dirInfo.Parent.FullName + "\\", "");
            filePath = filePath.Replace(Path.GetExtension(path), "");
            filePath = filePath.Replace("\\", "/");
            return filePath;
        }
        public class AutoGenResourcesList : UnityEditor.AssetModificationProcessor
        {

            public static void OnWillSaveAssets(string[] names)
            {
                foreach (string name in names)
                {
                    if (name.EndsWith(".unity"))
                    {
                        ResourcesListGen.UpdateResourcesList();
                        break;
                    }
                }

            }

        }
    }

}