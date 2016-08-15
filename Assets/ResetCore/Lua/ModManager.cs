#if LUA
using UnityEngine;
using System.Collections;
using ResetCore.Util;
using LuaInterface;
using System.Collections.Generic;
using System.IO;
using ResetCore.Asset;

namespace ResetCore.Lua
{
    public class ModManager : MonoBehaviour
    {

        private static List<LuaTable> luaTableList = new List<LuaTable>();

        /// <summary>
        /// 提供给外部手动执行LUA脚本的接口
        /// </summary>
        public void Initilize()
        {
            if (!Directory.Exists(PathConfig.localModLuaFilePath))
            {
                Debug.logger.Log(PathConfig.localModLuaFilePath);
                Directory.CreateDirectory(PathConfig.localModLuaFilePath);
            }
            string[] filePaths = Directory.GetFiles(PathConfig.localModLuaFilePath, "*.txt", SearchOption.AllDirectories);
            ActionQueue loadQueue = new ActionQueue();
            
            foreach (string path in filePaths)
            {
                loadQueue.AddAction((act) =>
                {
                    ResourcesLoaderHelper.Instance.LoadTextResourcesByPath("file:///" + path, (scr, bo) =>
                    {
                        if (bo)
                        {
                            luaTableList.Add(LuaManager.luaScrMgr.DoString(scr)[0] as LuaTable);
                            act();
                        }
                    });
                });
            }

            loadQueue.AddAction((act) =>
            {
                foreach (LuaTable LuaModule in luaTableList)
                {
                    CallLuaFunction("Init", LuaModule, LuaModule);
                }
            });
        }

        void Awake()
        {
            Initilize();
        }


        // MonoBehaviour callback
        void Update()
        {
            foreach (LuaTable LuaModule in luaTableList)
            {
                CallLuaFunction("Update", LuaModule, LuaModule);
            }
        }

        /// <summary>
        /// 调用一个Lua组件中的函数
        /// </summary>
        void CallLuaFunction(string funcName, LuaTable LuaModule, params object[] args)
        {
            
            if (LuaModule == null)
            {
                Debug.logger.LogError("Mod", "没有该LuaTabel");
                return;
            }
                

            LuaFunction func = LuaModule[funcName] as LuaFunction;
            if (func != null)
            {
                func.Call(args);
            }
            else
            {
                Debug.logger.LogError("Mod", "没有该函数" + funcName);
            }
                
        }

    }

}
#endif