using UnityEngine;
using System.Collections;
using ResetCore.Util;

namespace ResetCore.NGUI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField]
        private Camera camera;
        [SerializeField]
        private Transform normalUI;
        [SerializeField]
        private Transform popupUI;
        [SerializeField]
        private Transform topUI;
        

    }
}

