using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ResetCore.VersionControl
{
    public enum VERSION_SYMBOL
    {
        ASSET,
        EVENT,
        UTIL,

        AOP,
        BEHAVIOR_TREE,
        CSTOOL,
        DATA_GENER,
        DATA_STRUCT,
        DEBUG,
        DLLMANAGER,
        FSM,
        GAMESYSTEMS,
        IMPORT_HELPER,
        LUA,
        MYSQL,
        NETPOST,
        NGUI,
        OBJECT,
        PLATFORM_HELPER,
        SHADER,
        TEST,
        UGUI,
        XML,

        AR,
        VR_VIVE
    }

    public static class VersionConst
    {
        public static readonly List<VERSION_SYMBOL> defaultSymbol = new List<VERSION_SYMBOL>()
        {
            VERSION_SYMBOL.EVENT,
            VERSION_SYMBOL.ASSET,
            VERSION_SYMBOL.UTIL,
        };

        public static readonly string DeveloperSymbolName = "RESET_DEVELOPER";

        public static readonly Dictionary<VERSION_SYMBOL, string> SymbolName = new Dictionary<VERSION_SYMBOL, string>()
        {
            {VERSION_SYMBOL.ASSET,"ASSET"},
            {VERSION_SYMBOL.UTIL,"UTIL"},
            {VERSION_SYMBOL.EVENT,"EVENT"},

            {VERSION_SYMBOL.AOP,"AOP"},
            {VERSION_SYMBOL.BEHAVIOR_TREE,"BEHAVIOR_TREE"},
            {VERSION_SYMBOL.CSTOOL,"CSTOOL"},
            {VERSION_SYMBOL.DATA_GENER,"DATA_GENER"},
            {VERSION_SYMBOL.DATA_STRUCT,"DATA_STRUCT"},
            {VERSION_SYMBOL.DEBUG,"DEBUG"},
            {VERSION_SYMBOL.DLLMANAGER,"DLLMANAGER"},
            {VERSION_SYMBOL.FSM,"FSM"},
            {VERSION_SYMBOL.GAMESYSTEMS,"GAMESYSTEMS"},
            {VERSION_SYMBOL.IMPORT_HELPER,"IMPORT_HELPER"},
            {VERSION_SYMBOL.LUA,"LUA"},
            {VERSION_SYMBOL.MYSQL,"MYSQL"},
            {VERSION_SYMBOL.NETPOST,"NETPOST"},
            {VERSION_SYMBOL.NGUI,"NGUI"},
            {VERSION_SYMBOL.OBJECT,"OBJECT"},
            {VERSION_SYMBOL.PLATFORM_HELPER,"PLATFORMHELPER"},
            {VERSION_SYMBOL.SHADER,"SHADER"},
            {VERSION_SYMBOL.TEST,"TEST"},
            {VERSION_SYMBOL.UGUI,"UGUI"},
            {VERSION_SYMBOL.XML,"XML"},

            {VERSION_SYMBOL.AR,"AR"},
            {VERSION_SYMBOL.VR_VIVE,"VR_VIVE"},
        };

        public static readonly Dictionary<VERSION_SYMBOL, string> SymbolFoldNames = new Dictionary<VERSION_SYMBOL, string>()
        {
            {VERSION_SYMBOL.ASSET, "Core/Asset"},
            {VERSION_SYMBOL.UTIL, "Core/Util"},
            {VERSION_SYMBOL.EVENT,"Core/Events"},

            {VERSION_SYMBOL.AOP,"Aop"},
            {VERSION_SYMBOL.BEHAVIOR_TREE,"BehaviorTree"},
            {VERSION_SYMBOL.CSTOOL,"CSTool"},
            {VERSION_SYMBOL.DATA_GENER, "DataGener"},
            {VERSION_SYMBOL.DATA_STRUCT, "DataStruct"},
            {VERSION_SYMBOL.DEBUG, "Debug"},
            {VERSION_SYMBOL.DLLMANAGER, "DllManager"},
            {VERSION_SYMBOL.FSM,"FSM"},
            {VERSION_SYMBOL.GAMESYSTEMS, "GameSystems"},
            {VERSION_SYMBOL.IMPORT_HELPER, "ImportHelper"},
            {VERSION_SYMBOL.LUA, "Lua"},
            {VERSION_SYMBOL.MYSQL, "MySQL"},
            {VERSION_SYMBOL.NETPOST, "NetPost"},
            {VERSION_SYMBOL.NGUI, "NGUI"},
            {VERSION_SYMBOL.OBJECT, "Object"},
            {VERSION_SYMBOL.PLATFORM_HELPER, "PlatformHelper"},
            {VERSION_SYMBOL.SHADER, "Shader"},
            {VERSION_SYMBOL.TEST, "Test"},
            {VERSION_SYMBOL.UGUI, "UGUI"},
            {VERSION_SYMBOL.XML, "Xml"},

            {VERSION_SYMBOL.AR, "ARToolKit"},
            {VERSION_SYMBOL.VR_VIVE, "VR_Vive"},
        };
        //模块注释
        public static readonly Dictionary<VERSION_SYMBOL, string> SymbolComments = new Dictionary<VERSION_SYMBOL, string>()
        {
            {VERSION_SYMBOL.ASSET, "热更新基本框架以及资源管理"},
            {VERSION_SYMBOL.UTIL, "常用功能，包含大量常用扩展"},
            {VERSION_SYMBOL.EVENT,"事件分发器"},

            {VERSION_SYMBOL.AOP,"Aop扩展"},
            {VERSION_SYMBOL.BEHAVIOR_TREE,"行为树框架（开发中）"},
            {VERSION_SYMBOL.CSTOOL,"基于控制台的工具（开发中）"},
            {VERSION_SYMBOL.DATA_GENER, "游戏数据生成器"},
            {VERSION_SYMBOL.DATA_STRUCT, "数据结构（开发中）"},
            {VERSION_SYMBOL.DEBUG, "Debug扩展（开发中）"},
            {VERSION_SYMBOL.DLLMANAGER, "Dll管理器（开发中）"},
            {VERSION_SYMBOL.FSM,"有限状态机（来自github）"},
            {VERSION_SYMBOL.GAMESYSTEMS, "游戏系统"},
            {VERSION_SYMBOL.IMPORT_HELPER, "导入助手（来自Infinite Code）"},
            {VERSION_SYMBOL.LUA, "Lua扩展(开发中,使用前请先安装NeedPlugin中的插件)"},
            {VERSION_SYMBOL.MYSQL, "对MySQL进行支持（开发中）"},
            {VERSION_SYMBOL.NETPOST, "NetPost基本HTTP框架"},
            {VERSION_SYMBOL.NGUI, "NGUI基本UI框架, 已经包含NGUI插件"},
            {VERSION_SYMBOL.OBJECT, "游戏场景中对物体的控制"},
            {VERSION_SYMBOL.PLATFORM_HELPER, "对各个导出平台进行支持（开发中）"},
            {VERSION_SYMBOL.SHADER, "对Shader的扩展（开发中）"},
            {VERSION_SYMBOL.TEST, "测试模块"},
            {VERSION_SYMBOL.UGUI, "UGUI基本UI框架"},
            {VERSION_SYMBOL.XML, "对Xml的扩展支持"},

            {VERSION_SYMBOL.AR, "对ARToolKit的扩展支持（开发中,使用前请先安装NeedPlugin中的插件）"},
            {VERSION_SYMBOL.VR_VIVE, "对SteamVR的扩展支持（开发中,使用前请先安装NeedPlugin中的插件）"},
        };


        public static string GetSymbolPath(VERSION_SYMBOL symbol)
        {
            return Path.Combine(PathConfig.ResetCorePath, SymbolFoldNames[symbol]);
        }

        public static string GetSymbolTempPath(VERSION_SYMBOL symbol)
        {
            return Path.Combine(PathConfig.ResetCoreBackUpPath, SymbolFoldNames[symbol]);
        }

       
        
    }

}
