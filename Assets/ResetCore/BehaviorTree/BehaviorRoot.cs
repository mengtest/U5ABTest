using UnityEngine;
using System.Collections;
using ResetCore.Asset;
using System.Xml.Linq;
using System;
using System.Reflection;

public class BehaviorRoot : MonoBehaviour {

    [SerializeField]
    private string behaviorTreeInfoPath;

    private Behavior rootBehavior;

    private bool inited;

    void Awake()
    {
        inited = false;
        LoadBehaviorTreeInfo();
        rootBehavior.DoBehavior();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void LoadBehaviorTreeInfo()
    {
        string xmlStr = ResourcesLoaderHelper.Instance.LoadTextAsset(behaviorTreeInfoPath).text;
        XDocument xDoc = XDocument.Parse(xmlStr);

        string rootBehaviorName = xDoc.Root.Name.LocalName;
        rootBehavior = Behavior.Getbehavior(rootBehaviorName);
        rootBehavior.root = this;
        LoadBehaviorList(xDoc.Root, rootBehavior);
        inited = true;
    }

    private void LoadBehaviorList(XElement parentEl, Behavior parentBehavior)
    {
        if(!parentEl.HasElements) return;

        Behavior childBehavior;

        foreach (XElement el in parentEl.Elements())
        {

            childBehavior = Behavior.Getbehavior(el.Name.LocalName);
            parentBehavior.AddChild(childBehavior);

            if (el.HasElements)
            {
                LoadBehaviorList(el, childBehavior);
            }
        }
    }
}
