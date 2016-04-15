using UnityEngine;
using System.Collections;

namespace ResetCore.UGUI
{
    public class BaseNormalUI : BaseUI
    {

        protected override void Awake()
        {
            uiRoot = UIManager.Instance.normalRoot;
        }

    }
}


