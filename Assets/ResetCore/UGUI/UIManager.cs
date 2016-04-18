using UnityEngine;
using System.Collections;
using ResetCore.Util;
using System.Collections.Generic;
using ResetCore.Asset;


namespace ResetCore.UGUI
{
    public class ShowUIArg
    {

    }

    public class UIManager : MonoBehaviour
    {

        private static UIManager _instance;
        public static UIManager Instance
        {
            get 
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<UIManager>();
                }
                return _instance; 
            }
        }

        void Awake()
        {
            
        }

        void Start()
        {
            BaseUI[] uiGroup = normalRoot.GetComponentsInChildren<BaseUI>();
            foreach (BaseUI ui in uiGroup)
            {
                if (!uiDic.ContainsKey(ui.uiName))
                {
                    uiDic.Add(ui.uiName, ui);
                }
                else
                {
                    Destroy(ui.gameObject);
                }
                
            }
        }

        public Canvas canvas;

        public GameObject normalRoot;
        public GameObject popUpRoot;
        public GameObject topRoot;

        private Dictionary<UIConst.UIName, BaseUI> uiDic = new Dictionary<UIConst.UIName, BaseUI>();

        private readonly string prefabPath = "UIManager";

        public void ShowUI(UIConst.UIName name, ShowUIArg arg = null)
        {
            if (uiDic.ContainsKey(name))
            {
                uiDic[name].gameObject.SetActive(true);
                uiDic[name].transform.SetAsLastSibling();
                uiDic[name].Init(arg);
            }
            else
            {
                BaseUI newUI = ResourcesLoaderHelper.Instance.LoadAndGetInstance(UIConst.UIPrefabNameDic[name]).GetComponent<BaseUI>();
                newUI.Init(arg);
                uiDic.Add(name, newUI);
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

