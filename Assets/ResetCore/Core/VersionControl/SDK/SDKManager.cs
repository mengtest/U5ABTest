using UnityEngine;
using System.Collections;
using System.IO;
using ResetCore.Util;

namespace ResetCore.VersionControl
{
    public class SDKManager
    {
        //是否已经安装SDK
        public bool HasSetuped(SDKType sdkType)
        {
            string setupPath = VersionConst.GetSDKPath(sdkType);
            return Directory.Exists(setupPath);
        }

        //安装sdk
        public void SetupSDK(SDKType sdkType)
        {
            string backupPath = VersionConst.GetSDKBackupPath(sdkType);
            string setupPath = VersionConst.GetSDKPath(sdkType);
            string pluginPathBeforeSetup = Path.Combine(setupPath, PathConfig.PluginsFolderName);

            if (Directory.Exists(setupPath))
            {
                Debug.logger.LogError("SDK Setup Error", "You has setup the " + sdkType.ToString() + " SDK");
                return;
            }

            PathEx.MakeDirectoryExist(setupPath);
            DirectoryEx.DirectoryCopy(backupPath, setupPath, true);
            //安装Plugin
            if (Directory.Exists(pluginPathBeforeSetup))
            {
                Directory.Move(pluginPathBeforeSetup, PathConfig.pluginPath);
            }
        }
    }
}
