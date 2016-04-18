using UnityEngine;
using System.Collections;

namespace ResetCore.UGUI
{
    public class BaseNormalUI : BaseUI
    {

        protected override void OnEnable()
        {
            Debug.logger.Log(UIManager.Instance);
            uiRoot = UIManager.Instance.normalRoot;
            transform.SetParent(uiRoot.transform);
            transform.SetAsLastSibling();
            gameObject.ResetTransform();
        }

    }
}


