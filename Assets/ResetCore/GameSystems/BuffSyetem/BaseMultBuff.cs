using UnityEngine;
using System.Collections;
using ResetCore.Lua;

public class BaseMultBuff<T> : BaseBuff<T>
{

    public BaseMultBuff() { }
    public BaseMultBuff(BuffManager<T> manager, string luaName = null, float time = -1)
    {
        this.manager = manager;
        this.luaName = luaName;
        if (time < 0)
        {
            this.buffTime = (float)LuaManager.instance.GetValue<System.Double>(luaName, "BuffTime");
        }
    }

    public virtual void MultProperty()
    {
        if (luaName != null)
        {
            LuaManager.instance.Call(luaName, "DoBuff", manager);
        }
    }
}
