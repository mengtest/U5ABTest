using UnityEngine;
using System.Collections;
using System;

public class DoubleSpeedBuff : BaseBuff<PlayerProperty> {
    public override BuffType type
    {
        get
        {
            return BuffType.Mult;
        }
    }

    public override void BuffFunc(PlayerProperty effectObject)
    {
        effectObject.anim.SetBool("RunFast", true);
        effectObject.moveSpeed *= 2; 
    }
    public override void RemoveBuffFunc(PlayerProperty effectObject)
    {
        base.RemoveBuffFunc(effectObject);
        if(effectObject.anim != null)
            effectObject.anim.SetBool("RunFast", false);
    }
}
