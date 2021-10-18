using Autofac;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
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
using TurboA.AgileFramework.WebCore.AuthenticationExtend;
using TurboA.AgileFramework.WebCore.FilterExtend.SimpleExtend;
using TurboA.AgileFramework.WebCore.IOCExtend;
using TurboA.AgileFramework.WebCore.LogExtend;
using TurboA.AgileFramework.WebCore.MiddlewareExtend;
using TurboA.AgileFramework.WebCore.MiddlewareExtend.SimpleExtend;
using TurboA.AgileFramework.WebCore.MiddlewareExtend.StandardMiddleware;
using TurboA.AgileFramework.WebCore.RouteExtend;
using TurboA.AgileFramework.WebCore.StartupExtend;
using TurboA.NET6.DemoProject.CustomAuth;
using TurboA.NET6.DemoProject.Models;
using TurboA.NET6.DemoProject.Utility;
using TurboA.NET6.DemoProject.Utility.RouteExtend;
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
            //services.AddControllersWithViews().AddControllersAsServices();//服务化
            #region MVC配置
            //services.AddControllersWithViews();
            services.AddControllersWithViews(options =>
            {
                #region 顺序问题
                //options.Filters.Add<CustomGlobalOrderFilterAttribute>();//全局注册 对所有的Action都生效
                ////options.Filters.Add<CustomGlobalOrderFilterAttribute>(30);
                //options.Filters.Add(typeof(CustomGlobalOrderFilterAttribute), 30);
                #endregion

                #region Filter注入+ExceptionFilter
                //options.Filters.Add<CustomExceptionFilterAttribute>();
                #endregion
            })
            .AddRazorRuntimeCompilation()//动态编译
            .AddNewtonsoftJson()//返回中文
            .AddControllersAsServices()//服务化-Activetor
            ;

            #region ServiceFilter注入需要---CustomIOCFilterFactory注入需要
            services.AddScoped<CustomExceptionFilterAttribute>();
            #endregion

            #endregion

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

            #region 路由
            services.AddDynamicRoute();

            services.AddRouting(options =>
            {
                options.ConstraintMap.Add("GenderConstraint", typeof(CustomGenderRouteConstraint));
            });
            #endregion

            #region 鉴权授权  同时生效只有一个
            #region 基本鉴权流程---配置
            ////services.AddAuthentication();//没有任何Scheme不行，程序要求有DefaultScheme
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)//需要鉴权，且必须指定默认方案
            //         .AddCookie();//使用Cookie的方式

            //services.CustomAddAuthenticationCore();//替换下IOC,方便调试和扩展
            #endregion

            #region 自定义鉴权Handler
            ////使用Url参数传递用户信息
            //services.AddAuthentication(options =>
            //{
            //    options.AddScheme<UrlTokenAuthenticationHandler>(UrlTokenAuthenticationDefaults.AuthenticationScheme, "UrlTokenScheme-Demo");

            //    options.DefaultAuthenticateScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;//不能少
            //    options.DefaultChallengeScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultSignInScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultForbidScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultSignOutScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
            //});//覆盖默认注册,自定义解析Handler

            ////services.AddAuthentication(UrlTokenAuthenticationDefaults.AuthenticationScheme)
            ////.AddCookie()

            ////services.CustomAddAuthenticationCore();//替换下IOC,方便调试和扩展

            #endregion

            #region 自定义鉴权handler 堆叠Cookie--多handler
            services.AddAuthentication(options =>//覆盖默认注册,自定义解析Handler
            {
                options.AddScheme<UrlTokenAuthenticationHandler>(UrlTokenAuthenticationDefaults.AuthenticationScheme, "UrlTokenScheme-Demo");
                options.DefaultAuthenticateScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;//不能少
                options.DefaultChallengeScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
            })
               .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme) //再配置个Cookie解析
                ;
            services.CustomAddAuthenticationCore();//替换下IOC,方便调试和扩展
            #endregion

            #region 自定义鉴权handler 堆叠Cookie--多handler---加上授权策略
            //services.AddAuthentication(options =>//覆盖默认注册,自定义解析Handler
            //{
            //    options.AddScheme<UrlTokenAuthenticationHandler>(UrlTokenAuthenticationDefaults.AuthenticationScheme, "UrlTokenScheme-Demo");
            //    options.DefaultAuthenticateScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;//不能少
            //    options.DefaultChallengeScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultSignInScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultForbidScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultSignOutScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
            //})
            //   .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme) //再配置个Cookie解析
            //    ;
            //services.CustomAddAuthenticationCore();//替换下IOC,方便调试和扩展


            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("DateOfBirthPolicy", policyBuilder => policyBuilder.Requirements.Add(new DateOfBirthRequirement()));
            //    options.AddPolicy("CountryChinesePolicy", policyBuilder => policyBuilder.Requirements.Add(new CountryRequirement("Chinese")));
            //    options.AddPolicy("CountryChinaPolicy", policyBuilder => policyBuilder.Requirements.Add(new CountryRequirement("China")));

            //    options.AddPolicy("DoubleEmail", policyBuilder => policyBuilder.Requirements.Add(new DoubleEmailRequirement()));
            //});
            //services.AddSingleton<IAuthorizationHandler, ZhaoxiMailHandler>();
            //services.AddSingleton<IAuthorizationHandler, QQMailHandler>();

            //services.AddSingleton<IAuthorizationHandler, DateOfBirthRequirementHandler>();
            //services.AddSingleton<IAuthorizationHandler, CountryRequirementHandler>();
            #endregion

            #region 多handler  
            //同Scheme只能一个
            //services.AddAuthentication(options =>
            //    {
            //        options.AddScheme<UrlTokenAuthenticationHandler>(UrlTokenAuthenticationDefaults.AuthenticationScheme, "UrlTokenScheme-Demo");
            //        options.DefaultAuthenticateScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;//不能少
            //        options.DefaultChallengeScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
            //        options.DefaultSignInScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
            //        options.DefaultForbidScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;
            //        options.DefaultSignOutScheme = UrlTokenAuthenticationDefaults.AuthenticationScheme;

            //        options.RequireAuthenticatedSignIn = false;
            //    })
            //;
            //services.AddAuthentication(options =>
            // {
            //     options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme + "1";//不能少
            //     options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme + "1";
            //     options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme + "1";
            //     options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme + "1";
            //     options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme + "1";

            //     options.RequireAuthenticatedSignIn = false;
            // })
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme + "1", options =>
            //{
            //    options.LoginPath = "/Auth/Login1";
            //    options.Cookie.Name = "www1";
            //})
            //;
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme + "2", options =>
            //{
            //    options.LoginPath = "/Auth/Login2";
            //    options.Cookie.Name = "www2";
            //})
            //.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = "Zhaoxi.NET6.DemoProject",
            //        ValidAudience = "Zhaoxi.NET6.DemoProject",
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Eleven.Zhaoxi.NET6.DemoProject"))
            //    };
            //});
            #endregion

            #region Filter方式
            //services.AddAuthentication()
            //.AddCookie();
            #endregion

            #region 基于Cookies授权---最基础AddAuthorization--策略+角色授权
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;//不能少,signin signout Authenticate都是基于Scheme
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            //{
            //    options.LoginPath = "/Authorization/LoginPath";
            //    options.AccessDeniedPath = "/Authorization/AccessDeniedPath";
            //});
            ////services.AddAuthorization();//在AddController里面已经有了
            #endregion

            #region 替换IOC注册，理解流程---基于Cookies授权---最基础AddAuthorization--策略+角色授权
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;//不能少,signin signout Authenticate都是基于Scheme
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            //{
            //    options.LoginPath = "/Authorization/LoginPath";
            //    options.AccessDeniedPath = "/Authorization/AccessDeniedPath";
            //});
            ////services.AddAuthorization();//在AddController里面已经有了
            //services.CustomAddAuthorization();
            #endregion

            #region Cookie鉴权+Policy+Requirements+IAuthorizationHandler扩展
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;//不能少,signin signout Authenticate都是基于Scheme
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            //{
            //    options.LoginPath = "/Authorization/LoginPath";
            //    options.AccessDeniedPath = "/Authorization/AccessDeniedPath";
            //});

            ////定义一个共用的AuthorizationPolicy
            //var qqEmailPolicy = new AuthorizationPolicyBuilder().AddRequirements(new QQEmailRequirement()).Build();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminPolicy",
            //        policyBuilder => policyBuilder
            //        .RequireRole("Admin")//Claim的Role是Admin
            //        .RequireUserName("Eleven")//Claim的Name是Eleven
            //        .RequireClaim(ClaimTypes.Email)//必须有某个Cliam
            //        .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)//可以从这里解析
            //        .AddRequirements(new QQEmailRequirement())//QQ邮箱要求
            //        .Combine(qqEmailPolicy)//QQ邮箱要求  同上
            //        .RequireAssertion(context =>
            //            context.User.HasClaim(c => c.Type == ClaimTypes.Role)
            //            && context.User.Claims.First(c => c.Type.Equals(ClaimTypes.Role)).Value == "Admin")//根据授权处理上下文自定义规则检验
            //        );//内置

            //    options.AddPolicy("DoubleEmail", policyBuilder => policyBuilder.Requirements.Add(new DoubleEmailRequirement()));
            //});
            //services.AddSingleton<IAuthorizationHandler, ZhaoxiMailHandler>();
            //services.AddSingleton<IAuthorizationHandler, QQMailHandler>();
            #endregion

            #region 基于Cookie鉴权
            //services.AddScoped<ITicketStore, MemoryCacheTicketStore>();
            //services.AddMemoryCache();
            //////services.AddDistributedRedisCache(options =>
            //////{
            //////    options.Configuration = "127.0.0.1:6379";
            //////    options.InstanceName = "RedisDistributedSession";
            //////});
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;//不能少
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = "Cookie/Login";
            //})
            //.AddCookie(options =>
            //{
            //    //信息存在服务端--把key写入cookie--类似session
            //    options.SessionStore = services.BuildServiceProvider().GetService<ITicketStore>();
            //    options.Events = new CookieAuthenticationEvents()
            //    {
            //        OnSignedIn = new Func<CookieSignedInContext, Task>(
            //            async context =>
            //            {
            //                Console.WriteLine($"{context.Request.Path} is OnSignedIn");
            //                await Task.CompletedTask;
            //            }),
            //        OnSigningIn = async context =>
            //         {
            //             Console.WriteLine($"{context.Request.Path} is OnSigningIn");
            //             await Task.CompletedTask;
            //         },
            //        OnSigningOut = async context =>
            //        {
            //            Console.WriteLine($"{context.Request.Path} is OnSigningOut");
            //            await Task.CompletedTask;
            //        }
            //    };//扩展事件
            //});

            ////new AuthenticationBuilder().AddCookie()
            #endregion
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

                #region  扩展指定错误处理动作
                //app.UseStatusCodePagesWithReExecute("/Error/{0}");//只要不是200 都能进来
                //app.UseExceptionHandler(errorApp =>
                //{
                //    errorApp.Run(async context =>
                //    {
                //        context.Response.StatusCode = 200;
                //        context.Response.ContentType = "text/html";

                //        await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
                //        await context.Response.WriteAsync("ERROR!<br><br>\r\n");

                //        var exceptionHandlerPathFeature =
                //            context.Features.Get<IExceptionHandlerPathFeature>();

                //        Console.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
                //        Console.WriteLine($"{exceptionHandlerPathFeature?.Error.Message}");
                //        Console.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");

                //        // Use exceptionHandlerPathFeature to process the exception (for example, 
                //        // logging), but do NOT expose sensitive error information directly to 
                //        // the client.

                //        if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
                //        {
                //            await context.Response.WriteAsync("File error thrown!<br><br>\r\n");
                //        }

                //        await context.Response.WriteAsync("<a href=\"/\">Home</a><br>\r\n");
                //        await context.Response.WriteAsync("</body></html>\r\n");
                //        await context.Response.WriteAsync(new string(' ', 512)); // IE padding
                //    });
                //});
                //app.UseHsts();
                #endregion
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

            app.UseHttpsRedirection();

            #region 静态文件
            //app.UseRefuseStealing();

            app.UseStaticFiles();//静态文件处理中间件：处理静态文件的请求

            //app.UseRefuseStealing();//写在这里就没用了，因为前面已经处理了呀--很多中间件的顺序都是有原因的

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot"))
            //});//dotnet  和dotnet run

            //app.UseDirectoryBrowser(new DirectoryBrowserOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
            //    RequestPath = "/CsutomImages"
            //});
            #endregion

            #region Session
            //app.UseSession();//不是默认的，默认都没有Session，要用Session
            #endregion

            #region UseRouting
            app.UseRouting();
            #endregion

            #region between UseRouting and UseEndpoints
            ////其实就是缓存，可以更早缓存
            //app.Use(next =>
            //{
            //    Console.WriteLine("This is middleware between UseRouting and UseEndpoints");
            //    return new RequestDelegate(
            //        async context =>
            //        {
            //            await Task.Run(() =>
            //            {
            //                Console.WriteLine("************************************************************");
            //                Console.WriteLine("This is middleware between UseRouting and UseEndpoints start");
            //                //throw new Exception("middleware exception。。");

            //                var endpoint = context.GetEndpoint();

            //                //context.Features.Set<IEN>//还可以做映射处理，设置控制器action
            //                //或者额外加点参数信息 context.Items["aaa"] = "Eleven";
            //                if (endpoint is RouteEndpoint routeEndpoint)
            //                {
            //                    Console.WriteLine("Endpoint has route pattern: " +
            //                        routeEndpoint.RoutePattern.RawText);
            //                }
            //                if (endpoint != null)
            //                {
            //                    Console.WriteLine($"{endpoint.DisplayName}");
            //                    Console.WriteLine($"{string.Join(";", endpoint.Metadata)}");
            //                }
            //                else
            //                {
            //                    //没找到处理的  可以自行扩展处理动作--new一个404的处理
            //                }
            //            });
            //            await next.Invoke(context);
            //            await Task.Run(() =>
            //            {
            //                Console.WriteLine("This is middleware between UseRouting and UseEndpoints end");
            //                Console.WriteLine("************************************************************");
            //            });
            //        });
            //});
            #endregion

            #region 鉴权授权
            #region 默认就有
            //app.UseAuthorization();//默认就有--Add在AddController就已经添加了
            #endregion

            #region 标准组合
            app.UseAuthentication();//默认框架没有--就必须配套Add
            app.UseAuthorization();
            #endregion

            #region 替换组合
            //app.CustomUseAuthentication();
            //app.CustomUseAuthorization();
            #endregion

            #endregion

            #region 获取下路由匹配后的路由信息
            app.Use(next => context =>
            {
                var endpoint = context.GetEndpoint();
                if (endpoint is null)
                {
                    return next.Invoke(context);//没命中就继续
                    //return Task.CompletedTask;//没命中就无任何动作
                }

                Console.WriteLine($"Endpoint: {endpoint.DisplayName}");

                if (endpoint is RouteEndpoint routeEndpoint)
                {
                    Console.WriteLine(routeEndpoint.RoutePattern.Format());
                }

                foreach (var metadata in endpoint.Metadata)
                {
                    Console.WriteLine($"Endpoint has metadata: {metadata}");
                }
                return next.Invoke(context);
            });
            #endregion

            //Microsoft.AspNetCore.Mvc.Routing.DynamicRouteValueTransformer
            app.UseEndpoints(endpoints =>
            {
                #region 路由扩展
                endpoints.MapControllerRoute(
                name: "about-route",
                pattern: "about",
                defaults: new { controller = "Route", action = "About" }
                );//指向路由

                //endpoints.UseMapGetConstraint();

                endpoints.MapControllerRoute(
                name: "range",
                pattern: "{controller=Home}/{action=Index}/{year:range(2019,2021)}-{month:range(1,12)}");

                //伪静态
                //endpoints.MapControllerRoute(
                //    name: "static",
                //    pattern: "Item/{id:int}.html",
                //    defaults: new { controller = "Route", action = "PageInfo" });


                endpoints.MapControllerRoute(
                    name: "regular",
                    pattern: "{controller}/{action}/{year}-{month}",
                    constraints: new { year = "^\\d{4}$", month = "^\\d{2}$" },
                    defaults: new { controller = "Home", action = "Index", });

                endpoints.UseDynamicRouteDefault();

                #endregion

                //endpoints.MapAreaControllerRoute(
                //    name: "areas", "areas",
                //    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                       name: "default",
                       pattern: "{controller=Home}/{action=Index}/{id?}");

                //MapGet指定处理方式---MiniAPI
                endpoints.MapGet("/ElevenTest", async context =>
                {
                    await context.Response.WriteAsync($"This is ElevenTest");
                });
                //.RequireAuthorization();//要求授权
                //.WithMetadata(new AuditPolicyAttribute());//路由命中的话，可以多加个特性
            });
            #endregion
        }
    }
}
