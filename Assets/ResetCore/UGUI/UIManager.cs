using UnityEngine;
using System.Collections;
using ResetCore.Util;
using System.Collections.Generic;
using ResetCore.Asset;


namespace ResetCore.UGUI
{
    public class UIManager : MonoSingleton<UIManager>
    {

        public Canvas canvas;

        public GameObject normalRoot;
        public GameObject popUpRoot;
        public GameObject topRoot;

        private Dictionary<UIConst.UIName, BaseUI> uiDic = new Dictionary<UIConst.UIName, BaseUI>();

        private readonly string prefabPath = "UIManager";

        public override void Init()
        {
            base.Init();

            GameObject prefab = GameObject.Instantiate(Resources.Load(prefabPath)) as GameObject;
            Instance = prefab.GetComponent<UIManager>();
            DontDestroyOnLoad(gameObject);
        }

        public void ShowUI(UIConst.UIName name)
        {
            if (uiDic.ContainsKey(name))
            {
                uiDic[name].gameObject.SetActive(true);
                uiDic[name].transform.SetAsLastSibling();
            }
            else
            {
                BaseUI newUI = ResourcesLoaderHelper.Instance.LoadAndGetInstance(UIConst.UIPrefabNameDic[name]).GetComponent<BaseUI>();
                uiDic.Add(name, newUI);
                newUI.transform.parent = newUI.uiRoot.transform;
                uiDic[name].transform.SetAsLastSibling();
            }

        }

        public void HideUI(UIConst.UIName name)
        {
            if (uiDic.ContainsKey(name))
            {
                uiDic[name].gameObject.SetActive(false);
            }
        }

        public void CleanUI()
        {
            Destroy(normalRoot);
            normalRoot = new GameObject("NormalRoot");
            normalRoot.transform.parent = transform;

            Destroy(popUpRoot);
            popUpRoot = new GameObject("PopUpRoot");
            popUpRoot.transform.parent = transform;

            Destroy(topRoot);
            topRoot = new GameObject("TopRoot");
            topRoot.transform.parent = transform;

            uiDic.Clear();
        }
    }
}

