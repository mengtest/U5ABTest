using UnityEngine;
using System.Collections;
using ResetCore.Asset;
using System.Collections.Generic;

namespace ResetCore.Util
{
    public class ObjectPool : MonoSingleton<ObjectPool>
    {
        private static Dictionary<string, GameObject> poolDic;
        [HeaderAttribute("单个池内最大允许存在的物体数")]
        public int MaxSize = 30;
        [HeaderAttribute("单个池内清理下限")]
        public int CleanToSize = 15;

        [HeaderAttribute("当池内找不到是否搜索其他池")]
        public bool IsCheckAllPool = true;
        [HeaderAttribute("当隐藏时是否将池外物体加入池内")]
        public bool AddOutterObjectToPool = true;
        [HeaderAttribute("制作物体时是否重置其Transform")]
        public bool IsResetObject = false;


        public override void Init()
        {
            base.Init();
            poolDic = new Dictionary<string, GameObject>();
        }

        /// <summary>
        /// 池内寻找物体，若没有找到，则创建并加入池
        /// </summary>
        /// <param name="objectName">物体名</param>
        /// <param name="poolName">池名</param>
        /// <returns>返回的物体</returns>
        public GameObject CreateOrFindGameObject(string objectName, string poolName = "Defualt")
        {
            //检查池的存在性
            CheckPoolExist(poolName);

            //清理池
            CleanPool(poolName);

            GameObject finalGo = null;
            Transform poolTran = poolDic[poolName].transform;

            //寻找匹配物体
            finalGo = CheckPool(objectName, poolTran);

            //若没有找到，检查其他池
            if (finalGo == null && IsCheckAllPool)
            {
                finalGo = CheckAllPool(objectName, poolName);
            }

            //如果还是没有 那就创建
            if (finalGo == null)
            {
                finalGo = CreateObject(objectName);
                finalGo.transform.parent = poolTran;
            }

            if (finalGo == null)
            {
                return null;
            }
                
            finalGo.name = objectName;
            if (IsResetObject)
            {
                finalGo.ResetTransform();
            }

            finalGo.SetActive(true);

            return finalGo;
        }


        /// <summary>
        /// 隐藏或者删除物体，当AddOutterObjectToPool为true时加入池中，否则销毁
        /// </summary>
        /// <param name="go">要隐藏的物体</param>
        /// <param name="poolName">若在池外，则要加入的池名</param>
        public void HideOrDestroyObject(GameObject go, string poolName = "Defualt")
        {
            if (IsInPool(go))
            {
                go.SetActive(false);
            }
            else
            {
                if (AddOutterObjectToPool)
                {
                    CheckPoolExist(poolName);
                    go.transform.parent = poolDic[poolName].transform;
                    go.SetActive(false);
                }
                else
                {
                    Destroy(go);
                }
            }
        }

        /// <summary>
        /// 将物体加入某个池
        /// </summary>
        /// <param name="go">要加入的物体</param>
        /// <param name="poolName">所加入的池</param>
        public void AddObjectToPool(GameObject go, string poolName = "Defualt")
        {
            CheckPoolExist(poolName);

            if (go.transform.parent != null && go.transform.parent == poolDic[poolName].transform)
            {
                return;
            }

            go.transform.parent = poolDic[poolName].transform;
        }

        private void CheckPoolExist(string poolName)
        {
            if (!poolDic.ContainsKey(poolName))
            {
                GameObject newPool = new GameObject(poolName);
                newPool.transform.parent = transform;
                newPool.transform.position = Vector2.zero;
                poolDic.Add(poolName, newPool);
            }
        }

        private GameObject CheckPool(string objectName, Transform poolTran)
        {
            for (int i = 0; i < poolTran.childCount; i++)
            {
                Transform childTran = poolTran.GetChild(i);
                string childName = childTran.name;

                if (childName.Contains("(Clone)"))
                {
                    childName = childName.ReplaceFirst("(Clone)", "");
                }

                if (childName == objectName && childTran.gameObject.activeSelf == false)
                {
                    return childTran.gameObject;
                }
            }
            return null;
        }

        //检查所有池
        private GameObject CheckAllPool(string objectName, string poolToSkipName)
        {
            GameObject finalGo = null;
            foreach (GameObject pool in poolDic.Values)
            {
                if (pool != poolDic[poolToSkipName])
                {
                    finalGo = CheckPool(objectName, pool.transform);
                    if (finalGo != null)
                    {
                        return finalGo;
                    }
                }
            }
            return finalGo;
        }

        private bool IsInPool(GameObject go)
        {
            return (go.transform.parent != null && go.transform.parent.parent != null && go.transform.parent.parent == transform);
        }

        private GameObject CreateObject(string objectName)
        {
            GameObject newGo = ResourcesLoaderHelper.Instance.LoadAndGetInstance(objectName + ".prefab");

            return newGo;
        }

        private void CleanPool(string poolName)
        {
            Transform poolTran = poolDic[poolName].transform;
            int num = poolTran.childCount;
            if (num < MaxSize)
            {
                return;
            }

            for (int i = num - 1; i >= 0; i--)
            {
                if (num < CleanToSize)
                {
                    break;
                }

                Transform childTran = poolTran.GetChild(i);
                if (childTran.gameObject.activeSelf == false)
                {
                    Destroy(childTran.gameObject);
                    num--;
                }
            }
        }

        public void CleanAllPool()
        {
            transform.DoToAllChildren((child) =>
            {
                child.DeleteAllChild();
            });
        }

    }
}


