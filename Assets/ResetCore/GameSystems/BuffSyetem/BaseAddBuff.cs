using UnityEngine;
using System.Collections;
using ResetCore.Lua;

public class BaseAddBuff<T> : BaseBuff<T>
{

    public BaseAddBuff() { }
    public BaseAddBuff(BuffManager<T> manager, string luaName = null, float time = -1)
    {
        this.manager = manager;
        this.luaName = luaName;
        if (time < 0)
        {
            this.buffTime = (float)LuaManager.instance.GetValue<System.Double>(luaName, "BuffTime");
        }
    }

    public virtual void AddProperty()
    {
        if (luaName != null)
        {
            LuaManager.instance.Call(luaName, "DoBuff", manager);
        }
    }
}
