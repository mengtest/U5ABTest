﻿using UnityEngine;
using System.Collections;
using ResetCore.Util;
using System.Collections.Generic;
using ResetCore.Asset;


namespace ResetCore.UGUI
{


    public class UIManager : MonoSingleton<UIManager>
    {

        void Awake()
        {

        }

        void Start()
        {
            Camera.main.gameObject.AddComponent<CameraScale>();
            BaseUI[] uiGroup = canvas.GetComponentsInChildren<BaseUI>();
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

        [SerializeField]
        private Transform _normalRoot;
        public Transform normalRoot { get { return _normalRoot; } }
        [SerializeField]
        private Transform _popupRoot;
        public Transform popupRoot { get { return _popupRoot; } }
        [SerializeField]
        private Transform _topRoot;
        public Transform topRoot { get { return _topRoot; } }

        private Dictionary<UIConst.UIName, BaseUI> uiDic = new Dictionary<UIConst.UIName, BaseUI>();

        public void ShowUI(UIConst.UIName name, ShowUIArg arg = null, System.Action afterAct = null)
        {
            if (uiDic.ContainsKey(name))
            {
                if (uiDic[name] == null)
                {
                    BaseUI newUI = ResourcesLoaderHelper.Instance.LoadAndGetInstance(UIConst.UIPrefabNameDic[name]).GetComponent<BaseUI>();
                    uiDic[name] = newUI;
                }
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

            if (afterAct != null)
            {
                afterAct();
            }

        }

        public void HideUI(UIConst.UIName name, System.Action afterAct = null)
        {
            if (uiDic.ContainsKey(name) && uiDic[name] != null)
            {
                GetUI(name).Hide(afterAct);
            }
            if(afterAct != null)
            {
                afterAct();
            }
        }

        public BaseUI GetUI(UIConst.UIName name)
        {
            if (uiDic.ContainsKey(name))
            {
                if (uiDic[name] != null)
                {
                    return uiDic[name].gameObject.GetComponent<BaseUI>();
                }
                else
                {
                    uiDic.Remove(name);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public void CleanUI()
        {
            normalRoot.transform.DeleteAllChild();

            popupRoot.transform.DeleteAllChild();

            topRoot.transform.DeleteAllChild();

            uiDic.Clear();
        }

    }
}

