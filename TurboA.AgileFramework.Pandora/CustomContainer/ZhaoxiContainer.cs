using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Threading;
using Zhaoxi.AgileFramework.Pandora.CustomAOP;

namespace Zhaoxi.AgileFramework.Pandora.CustomContainer
{
    /// <summary>
    /// 是用来生成对象
    /// 第三方的  业务无关性
    /// </summary>
    public class ZhaoxiContainer : IZhaoxiContainer
    {
        private Dictionary<string, ZhaoxiContainerRegistModel> ZhaoxiContainerDictionary = new Dictionary<string, ZhaoxiContainerRegistModel>();
        private Dictionary<string, object[]> ZhaoxiContainerValueDictionary = new Dictionary<string, object[]>();
        /// <summary>
        /// 作用域单例的对象
        /// </summary>
        private Dictionary<string, object> ZhaoxiContainerScopeDictionary = new Dictionary<string, object>();

        private string GetKey(string fullName, string shortName) => $"{fullName}___{shortName}";


        public IZhaoxiContainer CreateChildContainer()
        {
            return new ZhaoxiContainer(this.ZhaoxiContainerDictionary, this.ZhaoxiContainerValueDictionary, new Dictionary<string, object>());//没有注册关系,最好能初始化进去
        }
        public ZhaoxiContainer() { }
        private ZhaoxiContainer(Dictionary<string, ZhaoxiContainerRegistModel> zhaoxiContainerDictionary,
            Dictionary<string, object[]> zhaoxiContainerValueDictionary, Dictionary<string, object> zhaoxiContainerScopeDictionary)
        {
            this.ZhaoxiContainerDictionary = zhaoxiContainerDictionary;
            this.ZhaoxiContainerValueDictionary = zhaoxiContainerValueDictionary;
            this.ZhaoxiContainerScopeDictionary = zhaoxiContainerScopeDictionary;
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
            this.ZhaoxiContainerDictionary.Add(this.GetKey(typeof(TFrom).FullName, shortName), new ZhaoxiContainerRegistModel()
            {
                Lifetime = lifetimeType,
                TargetType = typeof(TTo)
            });
            if (paraList != null && paraList.Length > 0)
                this.ZhaoxiContainerValueDictionary.Add(this.GetKey(typeof(TFrom).FullName, shortName), paraList);
        }
        public void RegisterType(Type typeFrom, Type typeTo, LifetimeType lifetimeType = LifetimeType.Transient)
        {
            this.ZhaoxiContainerDictionary.Add(this.GetKey(typeFrom.FullName, ""), new ZhaoxiContainerRegistModel()
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
            var model = this.ZhaoxiContainerDictionary[key];
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
                    if (this.ZhaoxiContainerScopeDictionary.ContainsKey(key))
                    {
                        return this.ZhaoxiContainerScopeDictionary[key];
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
            ctor = type.GetConstructors().FirstOrDefault(c => c.IsDefined(typeof(ZhaoxiConstructorAttribute), true));
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

            object[] paraConstant = this.ZhaoxiContainerValueDictionary.ContainsKey(key) ? this.ZhaoxiContainerValueDictionary[key] : null;//常量找出来
            int iIndex = 0;
            foreach (var para in ctor.GetParameters())
            {
                if (para.IsDefined(typeof(ZhaoxiParameterConstantAttribute), true))
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
            foreach (var prop in type.GetProperties().Where(p => p.IsDefined(typeof(ZhaoxiPropertyInjectionAttribute), true)))
            {
                Type propType = prop.PropertyType;
                string paraShortName = this.GetShortName(prop);
                object propInstance = this.ResolveObject(propType, paraShortName);
                prop.SetValue(oInstance, propInstance);
            }
            #endregion

            #region 方法注入 
            foreach (var method in type.GetMethods().Where(m => m.IsDefined(typeof(ZhaoxiMethodInjectionAttribute), true)))
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
                    this.ZhaoxiContainerScopeDictionary[key] = oInstance;
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
            if (provider.IsDefined(typeof(ZhaoxiParameterShortNameAttribute), true))
            {
                var attribute = (ZhaoxiParameterShortNameAttribute)(provider.GetCustomAttributes(typeof(ZhaoxiParameterShortNameAttribute), true)[0]);
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
        //    if (prop.IsDefined(typeof(ZhaoxiParameterShortNameAttribute), true))
        //    {
        //        return prop.GetCustomAttribute<ZhaoxiParameterShortNameAttribute>().ShortName;
        //    }
        //    else
        //        return null;
        //}
        //private string GetShortNamePara(ParameterInfo parameterInfo)
        //{
        //    if (parameterInfo.IsDefined(typeof(ZhaoxiParameterShortNameAttribute), true))
        //    {
        //        return parameterInfo.GetCustomAttribute<ZhaoxiParameterShortNameAttribute>().ShortName;
        //    }
        //    else
        //        return null;
        //}


    }
}
