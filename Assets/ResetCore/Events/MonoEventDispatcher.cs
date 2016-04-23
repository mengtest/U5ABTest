using UnityEngine;
using System.Collections;
using ResetCore.Event;
using System.Collections.Generic;

namespace ResetCore.Event
{
    public class MonoEventDispatcher
    {

        private static Dictionary<GameObject, EventController> monoEventControllerDict = new Dictionary<GameObject, EventController>();
        public static EventController GetMonoController(GameObject gameObject)
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

