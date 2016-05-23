using UnityEngine;
using System.Collections;

public abstract class BaseOtherBuff<T> : BaseBuff<T>
{

    public abstract void OtherEffect();
    public BaseOtherBuff() { }
    public BaseOtherBuff(BuffManager<T> manager)
    {
        this.manager = manager;
    }
	
}
