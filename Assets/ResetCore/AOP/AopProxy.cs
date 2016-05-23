using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;


namespace ResetCore.AOP
{

    interface IAopProceede
    {
        void PreProceede(IMessage msg);
        void PostProceede(IMessage msg);
    }

    //RealProxy
    public class AopProxy<T> : RealProxy 
    {
        private T _target;
        public AopProxy(T target)
            : base(typeof(T))
        {
            this._target = target;
        }
        public override IMessage Invoke(IMessage msg)
        {
            PreProceede(msg);
            IMethodCallMessage callMessage = (IMethodCallMessage)msg;
            object returnValue = callMessage.MethodBase.Invoke(this._target, callMessage.Args);
            PostProceede(msg);
            return new ReturnMessage(returnValue, new object[0], 0, null, callMessage);
        }
        public void PreProceede(IMessage msg)
        {
            UnityEngine.Debug.logger.Log("PreProceede");
        }
        public void PostProceede(IMessage msg)
        {
            UnityEngine.Debug.logger.Log("PostProceede");
        }

        public static T CreateProxy()
        {
            T instance = Activator.CreateInstance<T>();
            AopProxy<T> realProxy = new AopProxy<T>(instance);
            T transparentProxy = (T)realProxy.GetTransparentProxy();
            return transparentProxy;
        }
    }
}

