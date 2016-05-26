using UnityEngine;
using System.Collections;
using ResetCore.Lua;

public abstract class BaseBuff<T>
{

    public BuffManager<T> manager { protected get; set; }

    protected string luaName;
    public BaseBuff() { }
    public BaseBuff(BuffManager<T> manager, string luaName, float time = -1)
    {
        this.manager = manager;
        this.luaName = luaName;
        if (time < 0)
        {
            this.buffTime = (float)LuaManager.instance.GetValue<System.Double>(luaName, "BuffTime");
        }
    }

    public float buffTime { get; protected set; }
	
}
