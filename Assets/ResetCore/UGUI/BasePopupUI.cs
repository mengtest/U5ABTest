using UnityEngine;
using System.Collections;

namespace ResetCore.UGUI
{
    public class BasePopupUI : BaseUI
    {
        protected override void OnEnable()
        {
            uiRoot = UIManager.Instance.popUpRoot;
            transform.SetParent(uiRoot.transform);
            transform.SetAsLastSibling();
            gameObject.ResetTransform();
        }

    }

}

