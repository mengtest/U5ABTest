using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

public class ResourcesLoaderHelper{

    public Dictionary<string, string> resourcesList { get; private set; }

    private LocalResourcesLoader localLoader;
    private BundleResourcesLoader bundleLoader;
    public IResourcesLoad loader
    {
        get;
        protected set;
    }
    //单例模式
    public static ResourcesLoaderHelper Instance;
    public static ResourcesLoaderHelper instance
    {
        get
        {
            if (Instance == null)
            {
                Instance = new ResourcesLoaderHelper();
            }
            return Instance;
        }
    }

    public ResourcesLoaderHelper()
    {
        localLoader = new LocalResourcesLoader(this);
        bundleLoader = new BundleResourcesLoader(this);
        loader = bundleLoader;//默认设置为Bundle，当Bundle中无法找到时则切换为本地
        resourcesList = new Dictionary<string, string>();
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

    public IEnumerator LoadWWWAsset(string url, System.Action<Object> afterLoadAct, System.Action<float> processAct)
    {
        WWW www = new WWW(url);
        while (!www.isDone)
        {
            yield return www;
        }
        AssetBundle ab = www.assetBundle;
        if(ab != null)
        {

            //afterLoadAct(ab.LoadAsset());
        }
        
        
        
    }
	
}
