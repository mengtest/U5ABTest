using UnityEngine;
using System.Collections;

public enum BuffType
{
    Harmful,
    Beneficial
}

public abstract class BaseBuff<T>
{

    public BuffManager<T> manager { protected get; set; }

    public BaseBuff() { }
    public BaseBuff(BuffManager<T> manager)
    {
        this.manager = manager;
    }

    public float buffTime { get; protected set; }
    public abstract BuffType buffType { get; }
	
}
