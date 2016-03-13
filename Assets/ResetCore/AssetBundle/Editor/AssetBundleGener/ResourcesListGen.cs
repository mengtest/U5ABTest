using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Xml.Linq;
using System.IO;

namespace ResetCore.Asset
{
    

    public class ResourcesListGen
    {

        private static readonly string[] ignoreFliter = new string[] { ".meta", ".unity", ".shader" };

        [MenuItem("Tools/更新资源列表")]
        public static void UpdateResourcesList()
        {
            XDocument resourceListDoc = new XDocument(
                new XElement("Root")
                );
            XElement rootEl = resourceListDoc.Element("Root");
            DirectoryInfo resourceFolder = new DirectoryInfo(PathConfig.resourcePath);
            FileInfo[] fileInfos = resourceFolder.GetFiles("*", SearchOption.AllDirectories);

            foreach (FileInfo info in fileInfos)
            {
                string name = info.Name;
                string path = GetPathWithoutEx(info.FullName);

                //path = Path.GetFileNameWithoutExtension(path);

                if (IsResource(name))
                {
                    Debug.logger.Log("添加" + name);
                    rootEl.Add(new XElement("n", name));
                    rootEl.Add(new XElement("p", path));
                }

            }

            resourceListDoc.Save(PathConfig.resourcePath + PathConfig.resourceListDocPath + ".xml");
            AssetDatabase.Refresh();
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
            filePath = filePath.Replace("\\", "/");
            filePath = filePath.Replace(PathConfig.resourcePath, "");
            filePath = filePath.Replace(Path.GetExtension(path), "");
            return filePath;
        }

    }

}