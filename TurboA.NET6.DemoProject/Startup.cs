using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TurboA.AgileFramework.Pandora.IOCReplace;
using TurboA.AgileFramework.WebCore.IOCExtend;
using TurboA.AgileFramework.WebCore.LogExtend;
using TurboA.AgileFramework.WebCore.MiddlewareExtend;
using TurboA.AgileFramework.WebCore.MiddlewareExtend.SimpleExtend;
using TurboA.AgileFramework.WebCore.MiddlewareExtend.StandardMiddleware;
using TurboA.AgileFramework.WebCore.StartupExtend;
using TurboA.NET6.DemoProject.Models;
using TurboA.NET6.DemoProject.Utility;
using TurboA.NET6.Interface;
using TurboA.NET6.Service;

namespace TurboA.NET6.DemoProject
{
    /// <summary>
    /// 初始化配置
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllersWithViews();
            services.AddControllersWithViews().AddControllersAsServices();//服务化

            #region IOCShow
            ////只能构造函数注入--需要一个构造函数超集
            ////services.AddTransient<ITestServiceA, TestServiceA>();//瞬时
            //services.AddTransient<ITestServiceA, TestServiceAV2>();//瞬时
            //services.AddSingleton<ITestServiceB, TestServiceB>();//单例
            //services.AddScoped<ITestServiceC, TestServiceC>();//作用域单例--一次请求一个实例,
            ////作用域其实依赖于ServiceProvider（这个自身是根据请求的），跟多线程没关系
            //services.AddTransient<ITestServiceD, TestServiceD>();
            //services.AddTransient<ITestServiceE, TestServiceE>();
            #endregion

            #region IOCExtend
            //IControllerActivator的默认实现是DefaultControllerActivator
            //services.Replace(ServiceDescriptor.Transient<IControllerActivator, CustomControllerActivator>());//将容器注册好的IControllerActivator给换成自己的
            //原来映射关系都是这个容器管理--框架默认了一些处理方式--然后我们可以replace成自定义
            //记得添加AddControllersAsServices；

            //services.AddTransient<ITestServiceA, TestServiceA>();//瞬时
            //services.AddTransient<ITestServiceA, TestServiceAV2>();//瞬时
            //var a = services.BuildServiceProvider().GetService<ITestServiceA>();
            //var list = services.BuildServiceProvider().GetService<IEnumerable<ITestServiceA>>();

            //services.AddSingleton<ITestServiceA>(new TestServiceA());
            //services.AddSingleton<ITestServiceA>(new TestServiceAV2());
            //var list = services.BuildServiceProvider().GetService<IEnumerable<ITestServiceA>>();
            #endregion

            #region Options
            //services.Configure<EmailOption>(op => op.Title = "services.Configure<EmailOption>--DefaultName");//默认--名称empty
            //services.Configure<EmailOption>("FromMemory", op => op.Title = "services.Configure<EmailOption>---FromMemory");//指定名称,程序里面配置
            //services.Configure<EmailOption>("FromConfiguration", Configuration.GetSection("Email"));//从配置文件读取

            //services.Configure<EmailOption>("FromConfigurationNew", Configuration.GetSection("EmailNew"));//从配置文件读取

            //services.AddOptions<EmailOption>("AddOption").Configure(op => op.Title = "AddOption Title--DefaultName");//等价于Configure
            //services.Configure<EmailOption>(null, op => op.From = "services.Configure<EmailOption>--Name null--Same With ConfigureAll");
            ////services.ConfigureAll<EmailOption>(op => op.From = "ConfigureAll");

            //services.PostConfigure<EmailOption>(null, op => op.Body = "services.PostConfigure<EmailOption>--Name null--Same With PostConfigureAll");

            ////services.PostConfigureAll<EmailOption>(op => op.Body = "PostConfigurationAll");
            #endregion

            #region IStartupFilter拓展
            //services.AddTransient<IStartupFilter, CustomStartupFilter>();//ConfigureServices先执行的
            #endregion

            #region Middleware扩展
            //services.AddSingleton<SecondMiddleWare>();
            //services.Replace(ServiceDescriptor.Singleton<IMiddlewareFactory, SecondMiddleWareFactory>());//替换默认容器
            #endregion

            #region 标准套路
            ////services.AddBrowserFilter();//玩法1

            ////services.AddBrowserFilter(option =>
            ////{
            ////    option.EnableIE = false;
            ////    option.EnableFirefox = true;
            ////    option.EnableChorme = true;
            ////});//玩法2
            //services.AddBrowserFilter(option =>
            //{
            //    option.EnableIE = false;
            //    option.EnableFirefox = true;
            //    option.EnableChorme = true;
            //});//玩法3
            #endregion

            #region 分布式缓存
            //services.AddSession();//Session怎么用---组件化集中注册
            //services.AddDistributedRedisCache(options =>
            //{
            //    options.Configuration = "127.0.0.1:6379";
            //    options.InstanceName = "RedisDistributedCache";
            //});
            #endregion
        }

        #region 自定义容器
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="containerBuilder"></param>
        //public void ConfigureContainer(TurboAContainerBuilder containerBuilder)
        //{
        //    //containerBuilder.RegisterType<ITestServiceA, TestServiceA>();
        //    //containerBuilder.RegisterType<ITestServiceB, TestServiceB>();
        //    //containerBuilder.RegisterType<ITestServiceC, TestServiceC>();
        //    //containerBuilder.RegisterType<ITestServiceD, TestServiceD>();
        //    //containerBuilder.RegisterType<ITestServiceE, TestServiceE>();
        //}
        #endregion

        #region Autofac容器
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            //containerBuilder.RegisterType<TestServiceB>().As<ITestServiceB>().SingleInstance();
            containerBuilder.RegisterModule<CustomAutofacModule>();
        }
        #endregion

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 管道模型---承上启下---连接kestrel与MVC---Kestrel监听请求，解析得到HttpContext---MVC就是处理HttpContext(Request&Response)
        /// 连接点(管道)其实是个委托---RequestDelegate---接受HttpContext参数，处理后得到结果，都在HttpContext里面折腾
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            #region 最原生Use中间件
            ////代码有几层？一共有3层
            //app.Use(
            //   new Func<RequestDelegate, RequestDelegate>(
            //   next =>
            //   {
            //       Console.WriteLine("This is middleware 0.1");
            //       return new RequestDelegate(
            //            async context =>
            //       {
            //           await context.Response.WriteAsync("This is Hello World 0.1 start");
            //           await next.Invoke(context);
            //           await context.Response.WriteAsync("This is Hello World 0.1   end");

            //           await Task.Run(() => Console.WriteLine("12345678797989"));
            //       });
            //   }));

            //app.Use(
            //    new Func<RequestDelegate, RequestDelegate>(
            //    next =>
            //{
            //    Console.WriteLine("This is middleware 1");
            //    return new RequestDelegate(
            //        async context =>
            //        {
            //            await context.Response.WriteAsync("This is Hello World 1 start");
            //            //context.Response.OnStarting(state =>
            //            //{
            //            //    var httpContext = (HttpContext)state;
            //            //    httpContext.Response.Headers.Add("middleware", "12345");
            //            //    return Task.CompletedTask;
            //            //}, context);
            //            await next.Invoke(context);
            //            await context.Response.WriteAsync("This is Hello World 1   end");

            //            await Task.Run(() => Console.WriteLine("12345678797989"));
            //        });
            //}));

            //#region 随意增加
            //app.Use(next =>
            //{
            //    Console.WriteLine("This is middleware 1.5");
            //    return new RequestDelegate(
            //        async context =>
            //        {
            //            await context.Response.WriteAsync("This is Hello World 1.5 start");
            //            await next.Invoke(context);
            //            await context.Response.WriteAsync("This is Hello World 1.5   end");
            //        });
            //});
            //app.Use(next =>
            //{
            //    Console.WriteLine("This is middleware 1.6");
            //    return new RequestDelegate(
            //        async context =>
            //        {
            //            await context.Response.WriteAsync("This is Hello World 1.6 start");
            //            await next.Invoke(context);

            //        });
            //});
            //app.Use(next =>
            //{
            //    Console.WriteLine("This is middleware 1.7");
            //    return new RequestDelegate(
            //        async context =>
            //        {
            //            await next.Invoke(context);
            //            await context.Response.WriteAsync("This is Hello World 1.7   end");
            //        });
            //});
            //#endregion

            //app.Use(next =>
            //{
            //    Console.WriteLine("This is middleware 2");
            //    return new RequestDelegate(
            //        async context =>
            //        {
            //            await context.Response.WriteAsync("This is Hello World 2 start");
            //            await next.Invoke(context);
            //            await context.Response.WriteAsync("This is Hello World 2   end");
            //        });
            //});
            //app.Use(next =>
            //{
            //    Console.WriteLine("This is middleware 3");
            //    return new RequestDelegate(
            //        async context =>
            //        {
            //            await context.Response.WriteAsync("This is Hello World 3 start");
            //            //await next.Invoke(context);//最后这个没有执行Next---带着呢
            //            await context.Response.WriteAsync("This is The Chooen One!");
            //            await context.Response.WriteAsync("This is Hello World 3   end");
            //        });
            //});
            #endregion

            #region 各种内置扩展用法
            //app.Run(c => c.Response.WriteAsync("Hello World!"));//终结--不往下走
            //app.Use(next => c => c.Response.WriteAsync("Hello World!"));//终结式--Next未调用

            //app.Use(new Func<HttpContext, Func<Task>, Task>(async (context, next) =>//没有调用 next() 那就是终结点  跟Run一样
            //{
            //    await context.Response.WriteAsync("Hello World Use3  Again Again <br/>");
            //    //await next();
            //}));


            ////UseWhen可以对HttpContext检测后，增加处理环节;原来的流程还是正常执行的
            ////为啥这样？ 可以直接在中间件判断呀？满足就执行，不满足就继续--UseWhen并非如此
            ////这里面是弄了个全新的独立管道，又整合到整体里面去了，确实不一样，但是优势何在？
            //app.UseWhen(context =>
            //{
            //    return context.Request.Query.ContainsKey("Name");
            //},
            //appBuilder =>
            //{
            //    appBuilder.Use(new Func<HttpContext, Func<Task>, Task>(async (context, next) =>//没有调用 next() 那就是终结点  跟Run一样
            //    {
            //        await context.Response.WriteAsync("Hello World Use3  Again Again <br/>");
            //        //await next();
            //    }));
            //});

            ////根据条件指定中间件 指向终结点，没有Next
            ////Map是固定根据Path检测  MapWhen可以多条件
            //app.Map("/Test", MapTest);
            //app.Map("/TurboA", a => a.Run(async context =>
            //{
            //    await context.Response.WriteAsync($"This is Advanced TurboA Site");
            //}));
            //app.MapWhen(context =>
            //{
            //    return context.Request.Query.ContainsKey("Name");
            //                //拒绝非chorme浏览器的请求  
            //                //多语言
            //                //把ajax统一处理
            //            }, MapTest);

            //void MapTest(IApplicationBuilder app)
            //{
            //​               app.Run(async context =>
            //                { 
            //​                 await context.Response.WriteAsync("Url is " + context.Request.PathBase.ToString());
            //​               });
            //​             }
            #endregion

            #region UseMiddleware式
            ////UseMiddlerware 类--反射找
            //app.UseMiddleware<FirstMiddleWare>();
            //app.UseMiddleware<SecondMiddleWare>();
            //app.UseMiddleware<ThreeMiddleWare>("TurboA TurboA.NET6.DemoProject");
            #endregion

            #region 标准Middleware
            ////玩法1---Use传递---Add就无操作---IOptions<BrowserFilterOptions> options就是Use指定传递的 
            ////玩法2---Use不传递---靠Add实现---IOptions<BrowserFilterOptions> options就是IOC生成的
            ////玩法3---都传递---Use和Add都传递--Add为准1  Use为准2   叠加3---结果是2，以Use为准，原因是对象只会是UseMiddleware传递的值，就不会再找IOC了---但是合理吗？----可以升级注入IConfigureOptions<BrowserFilterOptions>，然后叠加生效

            ////app.UseBrowserFilter(new BrowserFilterOptions()
            ////{
            ////    EnableIE = true,
            ////    EnableFirefox = false
            ////});//玩法1

            ////app.UseBrowserFilter();//玩法2

            //app.UseBrowserFilter(new BrowserFilterOptions()
            //{
            //    EnableIE = true,
            //    EnableFirefox = false
            //});//玩法3
            //app.Use(next =>
            //{
            //    Console.WriteLine("This is middleware 3");
            //    return new RequestDelegate(
            //        async context =>
            //        {
            //            await context.Response.WriteAsync("This is Hello World 3 start");
            //            //await next.Invoke(context);//最后这个没有执行Next---带着呢
            //            await context.Response.WriteAsync("This is The Chooen One!");
            //            await context.Response.WriteAsync("This is Hello World 3   end");
            //        });
            //});
            #endregion

            #region Stream问题
          
            //这个不行
            //app.Use(next =>
            //{
            //    Console.WriteLine("This is middleware 3");
            //    return new RequestDelegate(
            //        async context =>
            //        {
            //            await context.Response.WriteAsync("This is Hello World 3 start");
            //            await next.Invoke(context);//最后这个没有执行Next---带着呢
            //            await context.Response.WriteAsync("This is The Chooen One!");
            //            await context.Response.WriteAsync("This is Hello World 3   end");
            //        });
            //});

            app.Use(
                new Func<RequestDelegate, RequestDelegate>(
                next =>
            {
                Console.WriteLine("This is middleware 1");
                return new RequestDelegate(
                    async context =>
                    {
                         Console.WriteLine("This is Hello World 1 start");
                        context.Response.OnStarting(state =>
                        {
                            var httpContext = (HttpContext)state;
                            httpContext.Response.Headers.Add("middleware", "12345");
                            //也不能去写response
                            return Task.CompletedTask;
                        }, context);
                        await next.Invoke(context);
                        Console.WriteLine("This is Hello World 1   end");
                    });
            }));

            //app.UseMiddleware<StreamReadMiddleware>();
            //app.UseMiddleware<StreamWriteMiddleware>();
            app.UseMiddleware<HeaderReadWriteMiddleware>();

            #endregion

            #region ASP.NET Core中间件
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            #region 添加Log4Net或者自定义的
            //loggerFactory.AddLog4Net();
            //loggerFactory.AddProvider(new CustomConsoleLoggerProvider())
            //loggerFactory.AddCustomConsoleLogger();

            //loggerFactory.AddCustomConsoleLogger(
            //    new CustomConsoleLoggerConfiguration
            //    {
            //        LogLevel = LogLevel.Debug,
            //        EventId = 0
            //    }) ;

            //loggerFactory.AddCustomConsoleLogger(
            //    c =>
            //{
            //    c.LogLevel = LogLevel.Debug;
            //    c.Init("TurboA");
            //});
            #endregion

            //app.UseHttpsRedirection();
            //app.UseSession();//要用Session
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion
        }
    }
}
