using UnityEngine;
using System.Collections;

namespace ResetCore.UGUI
{
    public class BasePopupUI : BaseUI
    {
        protected override void Awake()
        {
            uiRoot = UIManager.Instance.popUpRoot;
        }

    }

}

