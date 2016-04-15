using UnityEngine;
using System.Collections;


namespace ResetCore.UGUI
{
    public class BaseTopUI : BaseUI
    {
        protected override void Awake()
        {
            uiRoot = UIManager.Instance.topRoot;
        }


    }
}

