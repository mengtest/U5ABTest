using UnityEngine;
using System.Collections;
using ResetCore.Asset;
using System.Xml.Linq;
using System;
using System.Reflection;
using System.Collections.Generic;
using ResetCore.Event;
using ResetCore.Util;


namespace ResetCore.BehaviorTree
{
    public class BehaviorRoot : MonoBehaviour
    {

        //行为树路径
        [SerializeField]
        private string behaviorTreeInfoPath;

        //当触发这些事件时会进行Tick
        [SerializeField]
        private List<string> tickEventsList;

        private BaseBehaviorNode rootBehavior;


        public ActionNode currentRunningNode { get; set; }

        public ActionQueue actionQueue { get; private set; }

        void Awake()
        {
            LoadBehaviorTreeInfo();

            foreach (string eventName in tickEventsList)
            {
                MonoEventDispatcher.GetMonoController(gameObject).AddEventListener(eventName, Tick);
            }
        }

        void OnDestroy()
        {
            foreach (string eventName in tickEventsList)
            {
                MonoEventDispatcher.GetMonoController(gameObject).RemoveEventListener(eventName, Tick);
            }
        }


        void Start()
        {
            Tick();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LoadBehaviorTreeInfo()
        {
            string xmlStr = ResourcesLoaderHelper.Instance.LoadTextAsset(behaviorTreeInfoPath).text;
            XDocument xDoc = XDocument.Parse(xmlStr);

            string rootBehaviorName = xDoc.Root.Name.LocalName;
            rootBehavior = BaseBehaviorNode.Getbehavior(rootBehaviorName);
            rootBehavior.root = this;
            LoadBehaviorList(xDoc.Root, rootBehavior);
        }

        private void LoadBehaviorList(XElement parentEl, BaseBehaviorNode parentBehavior)
        {
            if (!parentEl.HasElements) return;

            BaseBehaviorNode childBehavior;

            foreach (XElement el in parentEl.Elements())
            {

                childBehavior = BaseBehaviorNode.Getbehavior(el.Name.LocalName);
                parentBehavior.AddChild(childBehavior);

                if (el.HasElements)
                {
                    LoadBehaviorList(el, childBehavior);
                }
            }
        }

        public void Tick()
        {
            actionQueue.Clean();
            currentRunningNode.StopBehavior();
            rootBehavior.DoBehavior();
        }
    }

}

