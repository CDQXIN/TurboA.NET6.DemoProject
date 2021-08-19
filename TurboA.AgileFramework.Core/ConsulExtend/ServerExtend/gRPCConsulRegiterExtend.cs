using NConsul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.AgileFramework.Core.ConsulExtend
{
    /// <summary>
    /// gRPC---当下注册模式时心跳失败
    /// consul发起请求--服务响应--但是没收到
    /// </summary>
    public static class gRPCConsulRegiterExtend
    {
        /// <summary>
        /// 自动读取配置文件完成注册
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static async Task UseGRPCConsulConfiguration(this IApplicationBuilder app, IConfiguration configuration)
        {
            ConsulRegisterOption consulRegisterOption = new ConsulRegisterOption();
            configuration.Bind("ConsulRegisterOption", consulRegisterOption);

            ConsulClientOption consulClientOption = new ConsulClientOption();
            configuration.Bind("ConsulClientOption", consulClientOption);

            await UseGRPCConsul(app, consulClientOption, consulRegisterOption);
        }
        /// <summary>
        /// 基于提供信息完成注册
        /// </summary>
        /// <param name="app"></param>
        /// <param name="consulClientOption"></param>
        /// <param name="consulRegisterOption"></param>
        /// <returns></returns>
        public static async Task UseGRPCConsul(this IApplicationBuilder app, ConsulClientOption consulClientOption, ConsulRegisterOption consulRegisterOption)
        {
            using (ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri($"http://{consulClientOption.IP}:{consulClientOption.Port}/");
                c.Datacenter = consulClientOption.Datacenter;
            }))
            {
                await client.Agent.ServiceRegister(new AgentServiceRegistration()
                {
                    ID = $"gRPC-{consulRegisterOption.IP}-{consulRegisterOption.Port}-{Guid.NewGuid()}",//唯一Id
                    Name = consulRegisterOption.GroupName,//组名称-Group
                    Address = consulRegisterOption.IP,
                    Port = consulRegisterOption.Port,
                    Tags = new string[] { consulRegisterOption.Tag },
                    Check = new AgentServiceCheck()//配置心跳检查的
                    {
                        Interval = TimeSpan.FromSeconds(consulRegisterOption.Interval),
                        Timeout = TimeSpan.FromSeconds(consulRegisterOption.Timeout),
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(consulRegisterOption.DeregisterCriticalServiceAfter),
                        GRPC = $"{consulRegisterOption.IP}:{consulRegisterOption.Port}",//直接gRPC
                        GRPCUseTLS = false
                    }
                });
                Console.WriteLine($"grpc://{consulRegisterOption.IP}:{consulRegisterOption.Port}完成注册");
            }
        }

    }
}
