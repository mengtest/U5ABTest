using UnityEngine;
using System.Collections;
using ResetCore.Event;
using System.Collections.Generic;

namespace ResetCore.Event
{
    public class MonoEventDispatcher
    {

        public static Dictionary<object, EventController> monoEventControllerDict = new Dictionary<object, EventController>();
        public static EventController GetMonoController(object gameObject)
        {
            if (gameObject == null) return null;

            if (!monoEventControllerDict.ContainsKey(gameObject))
            {
                monoEventControllerDict.Add(gameObject, new EventController());
            }
            return monoEventControllerDict[gameObject];
        }
    }
}

