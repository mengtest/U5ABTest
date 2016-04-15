using UnityEngine;
using System.Collections;

namespace ResetCore.UGUI
{
    public abstract class BaseUI : MonoBehaviour
    {

        public GameObject uiRoot { get; protected set; }

        protected virtual void Awake() { }

        protected virtual void OnEnable() { }

        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void OnDisable() { }
    }
}

