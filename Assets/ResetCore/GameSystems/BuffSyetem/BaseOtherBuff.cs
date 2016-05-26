using UnityEngine;
using System.Collections;
using ResetCore.Lua;

public class BaseOtherBuff<T> : BaseBuff<T>
{

    
    public BaseOtherBuff() { }
    public BaseOtherBuff(BuffManager<T> manager, string luaName = null, float time = -1)
    {
        this.manager = manager;
        this.luaName = luaName;
        if (time < 0)
        {
            this.buffTime = (float)LuaManager.instance.GetValue<System.Double>(luaName, "BuffTime");
        }
    }

    public virtual void OtherEffect()
    {
        if (luaName != null)
        {
            LuaManager.instance.Call(luaName, "DoBuff", manager);
        }
        
    }
	
}
