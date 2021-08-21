using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Threading;
using TurboA.AgileFramework.Pandora.CustomAOP;

namespace TurboA.AgileFramework.Pandora.CustomContainer
{
    /// <summary>
    /// 是用来生成对象
    /// 第三方的  业务无关性
    /// </summary>
    public class TurboAContainer : ITurboAContainer
    {
        private Dictionary<string, TurboAContainerRegistModel> TurboAContainerDictionary = new Dictionary<string, TurboAContainerRegistModel>();
        private Dictionary<string, object[]> TurboAContainerValueDictionary = new Dictionary<string, object[]>();
        /// <summary>
        /// 作用域单例的对象
        /// </summary>
        private Dictionary<string, object> TurboAContainerScopeDictionary = new Dictionary<string, object>();

        private string GetKey(string fullName, string shortName) => $"{fullName}___{shortName}";


        public ITurboAContainer CreateChildContainer()
        {
            return new TurboAContainer(this.TurboAContainerDictionary, this.TurboAContainerValueDictionary, new Dictionary<string, object>());//没有注册关系,最好能初始化进去
        }
        public TurboAContainer() { }
        private TurboAContainer(Dictionary<string, TurboAContainerRegistModel> TurboAContainerDictionary,
            Dictionary<string, object[]> TurboAContainerValueDictionary, Dictionary<string, object> TurboAContainerScopeDictionary)
        {
            this.TurboAContainerDictionary = TurboAContainerDictionary;
            this.TurboAContainerValueDictionary = TurboAContainerValueDictionary;
            this.TurboAContainerScopeDictionary = TurboAContainerScopeDictionary;
        }


        /// <summary>
        /// 加个参数区分生命周期--而且注册关系得保存生命周期
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="shortName"></param>
        /// <param name="paraList"></param>
        public void Register<TFrom, TTo>(string shortName = null, object[] paraList = null, LifetimeType lifetimeType = LifetimeType.Transient) where TTo : TFrom
        {
            this.TurboAContainerDictionary.Add(this.GetKey(typeof(TFrom).FullName, shortName), new TurboAContainerRegistModel()
            {
                Lifetime = lifetimeType,
                TargetType = typeof(TTo)
            });
            if (paraList != null && paraList.Length > 0)
                this.TurboAContainerValueDictionary.Add(this.GetKey(typeof(TFrom).FullName, shortName), paraList);
        }
        public void RegisterType(Type typeFrom, Type typeTo, LifetimeType lifetimeType = LifetimeType.Transient)
        {
            this.TurboAContainerDictionary.Add(this.GetKey(typeFrom.FullName, ""), new TurboAContainerRegistModel()
            {
                Lifetime = lifetimeType,
                TargetType = typeTo
            });

        }

        public TFrom Resolve<TFrom>(string shortName = null)
        {
            return (TFrom)this.ResolveObject(typeof(TFrom), shortName);
        }

        //private object oTestServiceA = null;
        /// <summary>
        /// 递归--可以完成不限层级的东西
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <returns></returns>
        private object ResolveObject(Type abstractType, string shortName = null)
        {
            string key = this.GetKey(abstractType.FullName, shortName);
            var model = this.TurboAContainerDictionary[key];
            #region Lifetime
            switch (model.Lifetime)
            {
                case LifetimeType.Transient:
                    Console.WriteLine("Transient Do Nothing Before~~");
                    break;
                case LifetimeType.Singleton:
                    if (model.SingletonInstance == null)
                    {
                        break;
                    }
                    else
                    {
                        return model.SingletonInstance;
                    }
                case LifetimeType.Scope:
                    if (this.TurboAContainerScopeDictionary.ContainsKey(key))
                    {
                        return this.TurboAContainerScopeDictionary[key];
                    }
                    else
                    {
                        break;
                    }
                    break;
                case LifetimeType.PerThread:
                    //CallContext  Remoting一个本地线程数据存储  .NetCore没有
                    object oValue = CustomCallContext<object>.GetData($"{key}{Thread.CurrentThread.ManagedThreadId}");
                    if (oValue == null)
                    {
                        break;
                    }
                    else
                    {
                        return oValue;
                    }
                default:
                    break;
            }
            #endregion

            Type type = model.TargetType;

            #region 选择合适的构造函数
            ConstructorInfo ctor = null;
            //2 标记特性
            ctor = type.GetConstructors().FirstOrDefault(c => c.IsDefined(typeof(TurboAConstructorAttribute), true));
            if (ctor == null)
            {
                //1 参数个数最多
                ctor = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).First();
            }
            //foreach (var c in type.GetConstructors())//参数个数最多
            //{

            //}
            //ctor = type.GetConstructors()[0];//直接第一个
            #endregion

            #region 准备构造函数的参数
            List<object> paraList = new List<object>();

            object[] paraConstant = this.TurboAContainerValueDictionary.ContainsKey(key) ? this.TurboAContainerValueDictionary[key] : null;//常量找出来
            int iIndex = 0;
            foreach (var para in ctor.GetParameters())
            {
                if (para.IsDefined(typeof(TurboAParameterConstantAttribute), true))
                {
                    paraList.Add(paraConstant[iIndex]);
                    iIndex++;
                }
                else
                {
                    Type paraType = para.ParameterType;//获取参数的类型 IUserDAL

                    string paraShortName = this.GetShortName(para);
                    object paraInstance = this.ResolveObject(paraType, paraShortName);
                    paraList.Add(paraInstance);
                }
            }
            #endregion

            object oInstance = null;
            oInstance = Activator.CreateInstance(type, paraList.ToArray());
            //if (oTestServiceA == null)
            //{
            //    oInstance = Activator.CreateInstance(type, paraList.ToArray());
            //    oTestServiceA = oInstance;
            //}
            //else
            //{
            //    oInstance = oTestServiceA;
            //}

            #region 属性注入
            foreach (var prop in type.GetProperties().Where(p => p.IsDefined(typeof(TurboAPropertyInjectionAttribute), true)))
            {
                Type propType = prop.PropertyType;
                string paraShortName = this.GetShortName(prop);
                object propInstance = this.ResolveObject(propType, paraShortName);
                prop.SetValue(oInstance, propInstance);
            }
            #endregion

            #region 方法注入 
            foreach (var method in type.GetMethods().Where(m => m.IsDefined(typeof(TurboAMethodInjectionAttribute), true)))
            {
                List<object> paraInjectionList = new List<object>();
                foreach (var para in method.GetParameters())
                {
                    Type paraType = para.ParameterType;//获取参数的类型 IUserDAL
                    string paraShortName = this.GetShortName(para);
                    object paraInstance = this.ResolveObject(paraType, paraShortName);
                    paraInjectionList.Add(paraInstance);
                }
                method.Invoke(oInstance, paraInjectionList.ToArray());
            }
            #endregion

            #region Lifetime
            switch (model.Lifetime)
            {
                case LifetimeType.Transient:
                    Console.WriteLine("Transient Do Nothing After~~");
                    break;
                case LifetimeType.Singleton:
                    model.SingletonInstance = oInstance;
                    break;
                case LifetimeType.Scope:
                    this.TurboAContainerScopeDictionary[key] = oInstance;
                    break;
                case LifetimeType.PerThread:
                    CustomCallContext<object>.SetData($"{key}{Thread.CurrentThread.ManagedThreadId}", oInstance);
                    break;
                default:
                    break;
            }
            #endregion

            return oInstance.AOP(abstractType);
            //return oInstance;
        }

        private string GetShortName(ICustomAttributeProvider provider)
        {
            if (provider.IsDefined(typeof(TurboAParameterShortNameAttribute), true))
            {
                var attribute = (TurboAParameterShortNameAttribute)(provider.GetCustomAttributes(typeof(TurboAParameterShortNameAttribute), true)[0]);
                return attribute.ShortName;
            }
            else
                return null;
        }

        public object Resolve(Type type)
        {
            return this.ResolveObject(type, null);
        }


        //private string GetShortNameProperty(PropertyInfo prop)
        //{
        //    if (prop.IsDefined(typeof(TurboAParameterShortNameAttribute), true))
        //    {
        //        return prop.GetCustomAttribute<TurboAParameterShortNameAttribute>().ShortName;
        //    }
        //    else
        //        return null;
        //}
        //private string GetShortNamePara(ParameterInfo parameterInfo)
        //{
        //    if (parameterInfo.IsDefined(typeof(TurboAParameterShortNameAttribute), true))
        //    {
        //        return parameterInfo.GetCustomAttribute<TurboAParameterShortNameAttribute>().ShortName;
        //    }
        //    else
        //        return null;
        //}


    }
}
