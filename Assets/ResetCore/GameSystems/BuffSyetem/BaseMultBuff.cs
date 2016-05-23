using UnityEngine;
using System.Collections;

public abstract class BaseMultBuff<T> : BaseBuff<T>
{

    public abstract void MultProperty();
    public BaseMultBuff() { }
    public BaseMultBuff(BuffManager<T> manager)
    {
        this.manager = manager;
    }
}
