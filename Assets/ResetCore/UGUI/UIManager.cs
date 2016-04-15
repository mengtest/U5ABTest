using UnityEngine;
using System.Collections;
using ResetCore.Util;
using System.Collections.Generic;

public class UIManager : MonoSingleton<UIManager> {

    private Canvas canvas;

    private GameObject normalRoot;
    private GameObject popUpRoot;
    private GameObject topRoot;

    public override void Init()
    {
        base.Init();
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        canvas.transform.parent = transform;

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.worldCamera = Camera.main;

        normalRoot = new GameObject("NormalRoot");
        normalRoot.transform.parent = canvas.transform;
        popUpRoot = new GameObject("PopUpRoot");
        popUpRoot.transform.parent = canvas.transform;
        topRoot = new GameObject("TopRoot");
        topRoot.transform.parent = canvas.transform;
    }

    public void ShowUI()
    {

    }

    public void HideUI()
    {

    }
}
