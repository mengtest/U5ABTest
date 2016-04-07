using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

public enum RunStatus
{
    Completed,
    Failure,
    Running,
}  

public abstract class Behavior 
{

    public RunStatus runState { get; set; }

    Behavior parent;
    List<Behavior> childBehaviorList = new List<Behavior>();
    public BehaviorRoot root { get; set; }
    

    public void AddChild(Behavior behavior)
    {
        behavior.parent = this;
        childBehaviorList.Add(behavior);
        behavior.root = root;
    }

    public void DeleteChild(Behavior behavior)
    {
        behavior.parent = null;
        childBehaviorList.Remove(behavior);
    }

    public abstract RunStatus DoBehavior();

    public static Behavior Getbehavior(string behaviorName)
    {
        Type rootBehaviorType = Type.GetType(behaviorName);

        ConstructorInfo constructor = rootBehaviorType.GetConstructor(new Type[] { });
        Behavior finalBehavior = constructor.Invoke(new object[] { }) as Behavior;
        return finalBehavior;
    }

}
