using UnityEngine;
using System.Collections;
using System.IO;

public class BundleResourcesLoader : IResourcesLoad {

    private ResourcesLoaderHelper LoadHelper;
    public ResourcesLoaderHelper loadHelper { get { return LoadHelper; } }

    public BundleResourcesLoader(ResourcesLoaderHelper helper)
    {
        LoadHelper = helper;
    }

    void IResourcesLoad.LoadResource(string objectName, System.Action<Object> afterLoadAct = null, System.Action<float> progressAct = null)
    {
        //string url = Path.Combine(PathConfig.bundlePath, ResourcesLoaderHelper.instance.resourcesList[objectName].Replace("\\", "/"));
        //loadHelper.LoadWWWAsset(url, afterLoadAct, progressAct);
        //Driver.instance.StartCoroutine(loadHelper.LoadWWWAsset(url, afterLoadAct, progressAct));
        string filePath = PathConfig.bundlePath + "/assets/resources/" + loadHelper.resourcesList[objectName];
        string mainPath = PathConfig.bundlePath + "/StandaloneWindows";
        AssetBundle assetBundle = AssetBundle.LoadFromFile(filePath);
        AssetBundle mainBundle = AssetBundle.LoadFromFile(mainPath);

        if (assetBundle != null && mainBundle != null)
        {
            Debug.logger.Log(filePath + ".manifest");
            AssetBundleManifest manifest = (AssetBundleManifest)mainBundle.LoadAsset("AssetBundleManifest");

            string[] dependenciesNames = manifest.GetAllDependencies("Cube");

            foreach (string depName in dependenciesNames)
            {
                Debug.logger.Log("depName" + depName);
            }

            Object obj = assetBundle.LoadAsset(objectName);
            mainBundle.Unload(false);
            afterLoadAct(obj);
        }
    }

    void IResourcesLoad.LoadResources(string[] objectsNames, System.Action<Object[]> afterLoadAct = null, System.Action<float> progressAct = null)
    {
        throw new System.NotImplementedException();
    }

    void IResourcesLoad.LoadAndGetInstance(string objectName, System.Action<GameObject> afterLoadAct = null, System.Action<float> progressAct = null)
    {
        throw new System.NotImplementedException();
    }
}
