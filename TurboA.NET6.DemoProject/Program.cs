using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Zhaoxi.AgileFramework.Pandora.IOCReplace;
using Zhaoxi.AgileFramework.WebCore.ConfigurationExtend;
using Zhaoxi.AgileFramework.WebCore.StartupExtend;

namespace Zhaoxi.NET6.DemoProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine(string.Join(",", args));//��ӡ�������в���
            var builder = CreateHostBuilder(args);
            var host = builder.Build();
            host.Run();

            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

        #region �����ļ�
                //.ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
                //{
                //    #region �ڴ�Provider
                //    var memoryConfig = new Dictionary<string, string>
                //    {
                //       {"TodayMemory", "0624-Memory"},
                //       {"RabbitMQOptions:HostName", "192.168.3.254-Memory"},
                //        {"RabbitMQOptions:UserName", "guest-Memory"},
                //         {"RabbitMQOptions:Password", "guest-Memory"}
                //    };
                //    configurationBuilder.AddInMemoryCollection(memoryConfig);
                //    #endregion

                //    #region Apollo�ֲ�ʽ��������
                //    //LogManager.UseConsoleLogging(Com.Ctrip.Framework.Apollo.Logging.LogLevel.Trace);
                //    //configurationBuilder
                //    //    .AddApollo(configurationBuilder.Build().GetSection("apollo"))
                //    //    .AddDefault()
                //    //    .AddNamespace("ZhaoxiMSAPrivateJson", ConfigFileFormat.Json)//�Զ����private NameSpace
                //    //    .AddNamespace(ConfigConsts.NamespaceApplication);//Apollo��Ĭ��NameSpace������
                //    #endregion

                //    #region XML
                //    configurationBuilder.AddXmlFile("appsettings.xml", optional: false, reloadOnChange: true);
                //    #endregion

                //    #region �Զ���ģʽ
                //    configurationBuilder.AddCustomConfiguration(option =>
                //    {
                //        option.LogTag = "This is CustomConfiguration";
                //        option.DataChangeAction = null;
                //        option.DataInitFunc = null;//δ�ṩ
                //    });
                //    #endregion
                //})
        #endregion

        #region ��չ��־
                //.ConfigureLogging((context, loggingBuilder) =>
                //{
                //    //loggingBuilder.ClearProviders();//��������Providers
                //    //loggingBuilder.AddConsole()
                //    //                .AddDebug()
                //    //                ;

                //    #region log4net
                //    loggingBuilder.AddFilter("System", LogLevel.Warning);//���˵������ռ�
                //    loggingBuilder.AddFilter("Microsoft", LogLevel.Warning);//
                //    loggingBuilder.AddLog4Net();//·����Ĭ��Ϊlog4net.config
                //    #endregion
                //})
        #endregion

        #region IOC����
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                //���ù������滻Ĭ�Ϲ���ʵ��---
                //.UseServiceProviderFactory(new ZhaoxiContainerFactory())
        #endregion

        #region ConfigureServices
                ////��ʱ��û��IOC���Ƿ���һ��ί��
                //    .ConfigureServices((context, services) =>
                //    {
                //        //services.AddTransient<IStartupFilter, CustomStartupFilter>();
                //        services.Configure<KestrelServerOptions>(
                //            context.Configuration.GetSection("Kestrel"));
                //    })//��ֱ��Startup��ConfigureServicesЧ����ȫһ��
        #endregion

        #region ConfigureWebHostDefaults
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    #region Kestrel����
                    //webBuilder.ConfigureKestrel(serverOptions =>
                    //{
                    //    serverOptions.Limits.MaxConcurrentConnections = 100;
                    //    serverOptions.Limits.MaxConcurrentUpgradedConnections = 100;
                    //    serverOptions.Limits.MaxRequestBodySize = 1024 * 1024;//byte--��IIS������ʧЧ��
                    //    serverOptions.Limits.MinRequestBodyDataRate = new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    //    serverOptions.Limits.MinResponseDataRate = new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));

                    //    serverOptions.Listen(IPAddress.Loopback, 8000);
                    //    serverOptions.Listen(IPAddress.Loopback, 9000);

                    //    serverOptions.Listen(IPAddress.Loopback, 9099, o => o.Protocols =
                    //         HttpProtocols.Http2);//�����в����Ķ˿ھ�ʧЧ��
                    //    //serverOptions.Listen(IPAddress.Loopback, 5001, listenOptions =>
                    //    // {
                    //    //     listenOptions.UseHttps("testCert.pfx", "testPassword");
                    //    // });//û�б���֤��


                    //    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
                    //    serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
                    //});
                    #endregion

                    webBuilder.UseStartup<Startup>();
                });
        #endregion
    }
}
