using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Zhaoxi.NET6.Interface;
using Zhaoxi.NET6.Service;
using Module = Autofac.Module;

namespace Zhaoxi.NET6.DemoProject.Utility
{
    public class CustomAutofacModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var builder = new ContainerBuilder();
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(assembly));
            manager.FeatureProviders.Add(new ControllerFeatureProvider());
            var feature = new ControllerFeature();
            manager.PopulateFeature(feature);
            builder.RegisterType<ApplicationPartManager>().AsSelf().SingleInstance();
            builder.RegisterTypes(feature.Controllers.Select(ti => ti.AsType()).ToArray()).PropertiesAutowired();

            containerBuilder.RegisterType<TestServiceA>().As<ITestServiceA>();
            containerBuilder.RegisterType<TestServiceB>().As<ITestServiceB>().SingleInstance();
            containerBuilder.RegisterType<TestServiceC>().As<ITestServiceC>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<TestServiceD>().As<ITestServiceD>();
            containerBuilder.RegisterType<TestServiceE>().As<ITestServiceE>();

            containerBuilder.Register(c => new CustomAutofacAop());//AOP注册
            containerBuilder.RegisterType<A>().As<IA>().EnableInterfaceInterceptors();
        }

    }
}
public class CustomAutofacAop : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        Console.WriteLine($"invocation.Methond={invocation.Method}");
        Console.WriteLine($"invocation.Arguments={string.Join(",", invocation.Arguments)}");

        invocation.Proceed(); //继续执行

        Console.WriteLine($"方法{invocation.Method}执行完成了");
    }
}

public interface IA
{
    void Show(int id, string name);
}

[Intercept(typeof(CustomAutofacAop))]
public class A : IA
{
    public void Show(int id, string name)
    {
        Console.WriteLine($"This is {id} _ {name}");
    }
}
