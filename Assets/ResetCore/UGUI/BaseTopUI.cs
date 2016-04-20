using UnityEngine;
using System.Collections;


namespace ResetCore.UGUI
{
    public class BaseTopUI : BaseUI
    {
        protected override void OnEnable()
        {
            uiRoot = UIManager.Instance.topRoot;
            transform.SetParent(uiRoot.transform, false);
            transform.SetAsLastSibling();
        }


    }
}

