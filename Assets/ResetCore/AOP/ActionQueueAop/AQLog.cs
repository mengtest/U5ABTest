using UnityEngine;
using System.Collections;

namespace ResetCore.AOP
{
    public static class AQLog
    {

        public static AQAopManager Log(this AQAopManager aqMgr, string bgMsg, string edMsg)
        {
            aqMgr.Add((act) =>
            {
                if (bgMsg != null)
                    Debug.logger.Log(bgMsg);
                act();
                if (edMsg != null)
                    Debug.logger.Log(edMsg);
            });
            return aqMgr;
        }

    }

}
