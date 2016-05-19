using UnityEngine;
using System.Collections;
using ResetCore.Util;
using LuaInterface;
using ResetCore.Asset;
using System.Collections.Generic;

namespace ResetCore.Lua
{
    public class LuaManager : Singleton<LuaManager>
    {

        private static LuaScriptMgr _luaScrMgr; // 全局的Lua虚拟机
        public static LuaScriptMgr luaScrMgr
        {
            get
            {
                if (_luaScrMgr == null)
                {
                    _luaScrMgr = new LuaScriptMgr();
                    _luaScrMgr.Start();
                }
                return _luaScrMgr;
            }
        }

        private Dictionary<string, string> luaDict = new Dictionary<string, string>();
        private Dictionary<string, LuaTable> luaTableDict = new Dictionary<string, LuaTable>();

        /// <summary>
        /// 运行Lua
        /// </summary>
        /// <param name="fileName"></param>
        public void DoLua(string fileName)
        {
            if (luaDict.ContainsKey(fileName))
            {
                luaScrMgr.DoString(luaDict[fileName]);
            }
            else
            {
                TextAsset textAsset = ResourcesLoaderHelper.Instance.LoadTextAsset(fileName + ".txt");
                luaScrMgr.DoString(textAsset.text);
                luaDict.Add(fileName, textAsset.text);
            }
        }

        /// <summary>
        /// 加载Lua内的模块
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public LuaTable LoadLua(string fileName)
        {
            if (luaTableDict.ContainsKey(fileName))
            {
                return luaTableDict[fileName];
            }
            else
            {
                TextAsset textAsset = ResourcesLoaderHelper.Instance.LoadTextAsset(fileName + ".txt");
                luaDict.Add(fileName, textAsset.text);
                LuaTable table = luaScrMgr.DoString(textAsset.text)[0] as LuaTable;
                luaTableDict.Add(fileName, table);
                return table;
            }

        }
        /// <summary>
        /// 调用Lua中的函数
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="funcName">函数明显</param>
        /// <param name="args">参数</param>
        public void Call(string fileName, string funcName, params object[] args)
        {
            if (luaTableDict[fileName] == null)
            {
                LoadLua(fileName);
            }

            LuaFunction func = this.luaTableDict[fileName][funcName] as LuaFunction;
            
            if (func != null)
            {
                func.Call(args);
            }
                
        }
    }
}

