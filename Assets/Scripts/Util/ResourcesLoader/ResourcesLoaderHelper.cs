using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

public class ResourcesLoaderHelper{

    public Dictionary<string, string> resourcesList = new Dictionary<string,string>();

    public ResourcesLoaderHelper()
    {
        XDocument resourcesListDoc = XDocument.Load(PathConfig.resourceListDocPath);
        int i = 1;
        string name = "";
        string path = "";
        foreach (XElement el in resourcesListDoc.Element("Root").Elements())
        {
            if (i % 2 == 0)
            {
                name = el.Value;
                Debug.Log(name);
            }
            else
            {
                path = el.Value;
                resourcesList.Add(name, path);
                Debug.Log(path);
            }
            i++;
        }
    }
	
}
