using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TurboA.AgileFramework.Pandora.CustomContainer;

namespace TurboA.AgileFramework.Pandora.IOCReplace
{
    /// <summary>
    /// 感兴趣可以把各种注册关系实现下
    /// </summary>
    public class TurboAContainerBuilder
    {
        private static ITurboAContainer _Container = new TurboAContainer();
        private static IServiceCollection _IServiceCollection = null;

        public TurboAContainerBuilder(IServiceCollection services)
        {
            _IServiceCollection = services;
            this.ServiceCollectionToTurboAContainer(services);
        }
        /// <summary>
        /// 把注册到默认容器的关系 转到自己容器
        /// </summary>
        /// <param name="services"></param>
        private void ServiceCollectionToTurboAContainer(IServiceCollection services)
        {

            foreach (var service in services)
            {
                #region 限定测试  下面处理都无效了
                string[] nameArray = new string[5] {
                "TurboA.NET6.Interface.ITestServiceA",
                "TurboA.NET6.Interface.ITestServiceB",
                "TurboA.NET6.Interface.ITestServiceC",
                "TurboA.NET6.Interface.ITestServiceD",
                "TurboA.NET6.Interface.ITestServiceE",
                };
                if (nameArray.Contains(service.ServiceType.FullName))
                {
                    _Container.RegisterType(service.ServiceType, service.ImplementationType);
                    Console.WriteLine($"将{service.ServiceType.Name}注册关系移到自定义容器");
                }
                continue;
                #endregion

                //按说是分类处理
                if (service.ImplementationFactory != null)
                {
                    //_Container.RegisterType(service.ServiceType, (provider => service.ImplementationFactory(provider)), this.TransLifetime(service.Lifetime));//尚未支持工厂注册
                }
                else if (service.ImplementationInstance != null)
                {
                    //_Container.RegisterType(service.ServiceType, service.ImplementationInstance);//尚未支持对象注册
                    _Container.RegisterType(service.ServiceType, service.GetType());
                }
                else
                {
                    _Container.RegisterType(service.ServiceType, service.ImplementationType, this.TransLifetime(service.Lifetime));
                }
            }
        }

        private LifetimeType TransLifetime(ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    return LifetimeType.Singleton;
                case ServiceLifetime.Scoped:
                    return LifetimeType.Scope;
                case ServiceLifetime.Transient:
                    return LifetimeType.Transient;
                default:
                    return LifetimeType.Transient;
            }
        }

        public void RegisterType<TFrom, TTo>(LifetimeType lifetimeType = LifetimeType.Transient) where TTo : TFrom
        {
            _Container.Register<TFrom, TTo>();
        }

        //public ITurboAContainer GetContainer()
        //{
        //    return _Container;
        //}

        /// <summary>
        /// 提供容器实例
        /// </summary>
        /// <returns></returns>
        public IServiceProvider GetServiceProvider()
        {
            return new TurboAServiceProvider(_Container);
            //return new TurboAServiceProvider(_Container, _IServiceCollection);
        }
    }
}
