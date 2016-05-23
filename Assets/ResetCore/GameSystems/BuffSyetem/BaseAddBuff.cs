using UnityEngine;
using System.Collections;

public abstract class BaseAddBuff<T> : BaseBuff<T>
{

    public abstract void AddProperty();
    public BaseAddBuff() { }
    public BaseAddBuff(BuffManager<T> manager)
    {
        this.manager = manager;
    }
}
