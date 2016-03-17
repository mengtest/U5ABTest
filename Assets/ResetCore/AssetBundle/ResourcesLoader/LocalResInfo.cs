using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;


namespace ResetCore.Asset
{
    public class LocalResInfo {

	    public string url { get; private set; }
        //被调用的时间
        public System.DateTime getTimeLastTime { get; private set; }
        public string localResName { get; private set; }

        private Object LocalRes;
        public Object localRes
        {
            get
            {
                getTimeLastTime = System.DateTime.Now;
                return LocalRes;
            }
            private set
            {
                LocalRes = value;
            }
        }


        public LocalResInfo(string resName)
        {
            url = ResourcesLoaderHelper.resourcesList[resName].Replace("Assets/Resources/", "");
            getTimeLastTime = System.DateTime.Now;
            localResName = resName;

            localRes = Resources.Load(url);

            if (localRes == null)
            {
                Debug.logger.LogError("LocalResInfo", url + "中的文件不存在！");
            }
        }

        public void Unload()
        {
            Resources.UnloadAsset(localRes);
        }
    }
}


