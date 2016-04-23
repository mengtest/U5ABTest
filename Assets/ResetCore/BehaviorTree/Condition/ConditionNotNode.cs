using UnityEngine;
using System.Collections;

public class ConditionNotNode : ConditionNode {

    protected override bool Handle()
    {
        return !childNode.DoBehavior();
    }
}
