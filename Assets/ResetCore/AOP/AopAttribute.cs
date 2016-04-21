using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Proxies;
using System;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Activation;
using System.Reflection;
using System.Runtime.Remoting.Contexts;

//public class AopAttribute : ProxyAttribute
//{
//    public override MarshalByRefObject CreateInstance(Type serverType)
//    {
//        AopProxy realProxy = new AopProxy(serverType);
//        return realProxy.GetTransparentProxy() as MarshalByRefObject;
//    }
//}


//class AopProxy : RealProxy
//{
//    Type serverType;
//    public AopProxy(Type serverType)
//        : base(serverType)
//    {
//        this.serverType = serverType;
//    }
//    public override IMessage Invoke(IMessage msg)
//    {
//        //消息拦截之后，就会执行这里的方法。
//        if (msg is IConstructionCallMessage) // 如果是构造函数，按原来的方式返回即可。
//        {
//            IConstructionCallMessage constructCallMsg = msg as IConstructionCallMessage;
//            IConstructionReturnMessage constructionReturnMessage = this.InitializeServerObject((IConstructionCallMessage)msg);
//            RealProxy.SetStubData(this, constructionReturnMessage.ReturnValue);
//            return constructionReturnMessage;
//        }
//        else if (msg is IMethodCallMessage) //如果是方法调用（属性也是方法调用的一种）
//        {
//            IMethodCallMessage callMsg = msg as IMethodCallMessage;
//            object[] args = callMsg.Args;
//            IMessage message;
//            try
//            {
//                var method = serverType.GetMethod("SetXX", BindingFlags.NonPublic | BindingFlags.Instance);
//                if (callMsg.MethodName.StartsWith("set_") && args.Length == 1)
//                {
//                    //这里检测到是set方法，然后应怎么调用对象的其它方法呢？
//                    method.Invoke(GetUnwrappedServer(), new object[] { callMsg.MethodName.Substring(4), args[0] });//对属性进行调用
//                }
//                object o = callMsg.MethodBase.Invoke(GetUnwrappedServer(), args);
//                message = new ReturnMessage(o, args, args.Length, callMsg.LogicalCallContext, callMsg);
//            }
//            catch (Exception e)
//            {
//                message = new ReturnMessage(e, callMsg);
//            }
//            return message;
//        }
//        return msg;
//    }


//}

