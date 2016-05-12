using UnityEngine;
using System.Collections;
using ResetCore.Util;
using System.Collections.Generic;
using System;

namespace ResetCore.NetPost
{
    /// <summary>
    /// 网络任务分发器，负责分发网络任务
    /// </summary>
    public class NetTaskDispatcher : Singleton<NetTaskDispatcher>
    {

        private Dictionary<string, ActionQueue> taskTable;



        public NetTaskDispatcher()
        {
            taskTable = new Dictionary<string, ActionQueue>();
        }

        public void AddNetPostTask(NetPostTask task, string queueName = "Defualt")
        {
            Action<Action> postAct = (act) =>
            {
                task.Start(act);
            };
            GetQueue(queueName).AddAction(postAct);
        }


        private ActionQueue GetQueue(string queueName)
        {
            if (!taskTable.ContainsKey(queueName))
            {
                taskTable.Add(queueName, new ActionQueue());
            }
            return taskTable[queueName];
        }
    }

}
