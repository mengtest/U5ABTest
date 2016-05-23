using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ResetCore.Util;

public class BuffManager<T> {

    private List<BaseAddBuff<T>> addBuffList = new List<BaseAddBuff<T>>();
    private List<BaseMultBuff<T>> multBuffList = new List<BaseMultBuff<T>>();
    private List<BaseOtherBuff<T>> otherBuffList = new List<BaseOtherBuff<T>>();

    public virtual void InitProperty()
    {

    }

    public void AddBuff(BaseBuff<T> buff)
    {
        buff.manager = this;
        if (buff is BaseAddBuff<T>)
        {
            BaseAddBuff<T> addBuff = buff as BaseAddBuff<T>;
            addBuffList.Add(addBuff);
            if(buff.buffTime > 0)
            {
                CoroutineTaskManager.Instance.WaitSecondTodo(() =>
                {
                    addBuffList.Remove(addBuff);
                    Recalculate();
                }, buff.buffTime);
            }
            
        }
        if (buff is BaseMultBuff<T>)
        {
            BaseMultBuff<T> multBuff = buff as BaseMultBuff<T>;
            multBuffList.Add(multBuff);
            if (buff.buffTime > 0)
            {
                CoroutineTaskManager.Instance.WaitSecondTodo(() =>
                {
                    multBuffList.Remove(multBuff);
                    Recalculate();
                }, buff.buffTime);
            }
        }
        if (buff is BaseOtherBuff<T>)
        {
            BaseOtherBuff<T> otherBuff = buff as BaseOtherBuff<T>;
            otherBuffList.Add(otherBuff);
            if (buff.buffTime > 0)
            {
                CoroutineTaskManager.Instance.WaitSecondTodo(() =>
                {
                    otherBuffList.Remove(otherBuff);
                    Recalculate();
                }, buff.buffTime);
            }
        }
        Recalculate();
    }

    private void Recalculate()
    {
        InitProperty();
        foreach (BaseAddBuff<T> buff in addBuffList)
        {
            buff.AddProperty();
        }
        foreach (BaseMultBuff<T> buff in multBuffList)
        {
            buff.MultProperty();
        }
        foreach (BaseOtherBuff<T> buff in otherBuffList)
        {
            buff.OtherEffect();
        }
    }
	
}
