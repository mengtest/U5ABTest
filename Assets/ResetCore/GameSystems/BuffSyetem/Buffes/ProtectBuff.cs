using UnityEngine;
using System.Collections;
using System;

public class ProtectBuff : BaseBuff<PlayerProperty> 
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
        effectObject.isProtected = true;
        effectObject.anim.SetBool("Invincible", true);
    }

    public override void RemoveBuffFunc(PlayerProperty effectObject)
    {
        base.RemoveBuffFunc(effectObject);
        effectObject.anim.SetBool("Invincible", false);
    }
}
