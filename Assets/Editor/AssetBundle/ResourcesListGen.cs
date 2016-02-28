using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Xml.Linq;
using System.IO;

public class ResourcesListGen {

    private static readonly string resourceListDocPath = Application.dataPath + "/Resources/Data/Common/ResourcesList.xml";
    private static readonly string resourcePath = Application.dataPath + "/Resources";
    private static readonly string[] ignoreFliter = new string[] { ".meta", ".unity", ".shader", ".xml"};

	[MenuItem("AssetBundle/生成本地资源列表")]
    public static void UpdateResourcesList()
    {
        XDocument resourceListDoc = new XDocument(
            new XElement("Root")
            );
        XElement rootEl = resourceListDoc.Element("Root");
        DirectoryInfo resourceFolder = new DirectoryInfo(resourcePath);
        FileInfo[] fileInfos = resourceFolder.GetFiles("*", SearchOption.AllDirectories);

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
            
        }

        resourceListDoc.Save(resourceListDocPath);
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
        filePath = filePath.Replace(resourcePath + "/", "");
        filePath = filePath.Replace(Path.GetExtension(path), "");
        return filePath;
    }

}
