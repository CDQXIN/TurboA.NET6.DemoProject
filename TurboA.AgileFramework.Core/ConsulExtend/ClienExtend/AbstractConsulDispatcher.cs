using Consul;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboA.AgileFramework.Core.ConsulExtend
{
    public abstract class AbstractConsulDispatcher
    {
        protected ConsulClientOption _ConsulClientOption = null;
        protected KeyValuePair<string, AgentService>[] _CurrentAgentServiceDictionary = null;

        public AbstractConsulDispatcher(IOptionsMonitor<ConsulClientOption> consulClientOption)
        {
            this._ConsulClientOption = consulClientOption.CurrentValue;
        }

        /// <summary>
        /// 负载均衡获取地址
        /// </summary>
        /// <param name="mappingUrl">Consul映射后的地址</param>
        /// <returns></returns>
        public string GetAddress(string mappingUrl)
        {
            Uri uri = new Uri(mappingUrl);
            string serviceName = uri.Host;
            string addressPort = this.ChooseAddress(serviceName);
            return $"{uri.Scheme}://{addressPort}{uri.PathAndQuery}";
        }

        protected virtual string ChooseAddress(string serviceName)
        {
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri($"http://{this._ConsulClientOption.IP}:{this._ConsulClientOption.Port}/");
                c.Datacenter = this._ConsulClientOption.Datacenter;
            });
            AgentService agentService = null;
            var response = client.Agent.Services().Result.Response;
            //foreach (var item in response)
            //{
            //    Console.WriteLine("***************************************");
            //    Console.WriteLine(item.Key);
            //    var service = item.Value;
            //    Console.WriteLine($"{service.Address}--{service.Port}--{service.Service}");
            //    Console.WriteLine("***************************************");
            //}

            this._CurrentAgentServiceDictionary = response.Where(s => s.Value.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase)).ToArray();


            int index = this.GetIndex();
            agentService = this._CurrentAgentServiceDictionary[index].Value;

            return $"{agentService.Address}:{agentService.Port}";
        }

        protected abstract int GetIndex();
    }
}
