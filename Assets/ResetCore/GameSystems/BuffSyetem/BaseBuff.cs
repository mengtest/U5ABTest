using UnityEngine;
using System.Collections;

public enum BuffType
{
    Harmful,
    Beneficial
}

public abstract class BaseBuff<T> {

    public BuffManager<T> manager { private get; set; }

    public abstract float buffTime { get; }
    public abstract BuffType buffType { get; }
	
}
