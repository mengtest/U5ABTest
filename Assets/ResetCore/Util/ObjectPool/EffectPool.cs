using UnityEngine;
using System.Collections;
using ResetCore.Util;
using ResetCore.Asset;
using System.Collections.Generic;

namespace ResetCore.Util
{
    public class EffectPool : MonoSingleton<EffectPool>
    {

        private List<GameObject> EfPool;

        public override void Init()
        {
            base.Init();
            EfPool = new List<GameObject>();
        }

        public GameObject PlayEffectInRoot(string efName, Vector3 pos)
        {
            GameObject efGo = FindOrCreateObject(efName);
            efGo.transform.position = pos;
            Play(efGo);
            return efGo;
        }

        public GameObject PlayEffectUnderTran(string efName, Transform tran, Vector3 localPos)
        {
            GameObject efGo = FindOrCreateObject(efName);
            efGo.transform.parent = tran;
            efGo.transform.localPosition = localPos;
            Play(efGo);
            return efGo;
        }

        public void HideEffect(GameObject go)
        {
            if (!EfPool.Contains(go))
            {
                Debug.logger.LogWarning("隐藏特效", "特效" + go.name + "不属于特效池");
                return;
            }
            go.SetActive(false);
        }

        private GameObject FindOrCreateObject(string efName)
        {
            GameObject efGo = null;
            efGo = CheckPool(efName);
            if (efGo == null)
            {
                efGo = CreateObject(efName);
                EfPool.Add(efGo);
            }
            return efGo;
        }

        private GameObject CheckPool(string efName)
        {
            foreach (GameObject go in EfPool)
            {
                string goName = go.name;
                if (goName.Contains("(Clone)"))
                {
                    goName = goName.ReplaceFirst("(Clone)", "");
                }

                if (goName == efName && go.activeSelf == false)
                {
                    return go;
                }
            }
            return null;
        }

        private GameObject CreateObject(string efName)
        {
            GameObject newGameObject = ResourcesLoaderHelper.Instance.LoadAndGetInstance(efName + ".prefab");
            return newGameObject;
        }

        private void Play(GameObject ef)
        {
            //DCTODO
            //anim.Play();
        }
    }
}

