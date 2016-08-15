#if AR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ResetCore.AR
{
    public class ARManager : MonoBehaviour
    {

        public ARController arController;
        public ARMarker arMarker;

        public AROrigin arOrigin;
        public ARCamera arCamera;

        public List<ARTrackedObject> arTrackedObjectList;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
#endif