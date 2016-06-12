using UnityEngine;
using System.Collections;
using ResetCore.Util;
using System;
using System.Collections.Generic;

namespace ResetCore.AOP
{
    public class AQAopManager
    {

        private Queue<Action> queue;

        private AQAopManager()
        {
            queue = new Queue<Action>();
        }

        public static AQAopManager Aop { get { return new AQAopManager(); } }

        public AQAopManager Add(Action<Action> act)
        {
            Action callback = () =>
            {
                queue.Dequeue().Invoke();
            };
            queue.Enqueue(() => { act(callback); });
            return this;
        }

        public AQAopManager Work(Action act)
        {
            queue.Enqueue(act);

            return this;
        }

        public void Submit()
        {
            queue.Dequeue().Invoke();
        }
    }
}

