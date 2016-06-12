using UnityEngine;
using System.Collections;

namespace ResetCore.AOP
{
    public static class AQTime
    {

        public static AQAopManager ShowUseTime(this AQAopManager aqMgr)
        {
            aqMgr.Add((act) =>
            {
                float startTime = Time.realtimeSinceStartup;
                act();
                float useTime = Time.realtimeSinceStartup - startTime;
                Debug.logger.Log("一共用时" + useTime);
            });
            return aqMgr;
        }
    }

}
