using UnityEngine;
using System.Collections;

public class FlyBuff : BaseBuff<PlayerProperty>
{
    public override BuffType type
    {
        get
        {
            return BuffType.Other;
        }
    }

    public override void BuffFunc(PlayerProperty effectObject)
    {
        effectObject.controllable = false;
        effectObject.gravityScale = 0;
        effectObject.collable = false;
        effectObject.isProtected = true;
        effectObject.anim.SetBool("RunHappily", true);
        effectObject.moveSpeed *= effectObject.airSpeedFactor;
    }
    public override void RemoveBuffFunc(PlayerProperty effectObject)
    {
        base.RemoveBuffFunc(effectObject);
        if (effectObject.anim != null)
            effectObject.anim.SetBool("RunHappily", false);
    }
}
