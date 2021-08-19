using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhaoxi.NET6.DemoProject.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IConfiguration _iConfiguration = null;
        public ConfigurationController(IConfiguration configuration)
        {
            this._iConfiguration = configuration;
        }

        /// <summary>
        /// http://localhost:5726/Configuration/Index
        /// 关闭kestrel相关配置
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            string AllowedHosts = this._iConfiguration["AllowedHosts"];
            //string allowedHost = this._iConfiguration["AllowedHost"].ToString();//异常

            string today = this._iConfiguration["Today"];
            string writeConn = this._iConfiguration["ConnectionStrings:Write"];
            string readConn0 = this._iConfiguration["ConnectionStrings:Read:0"];
            string[] _SqlConnectionStringRead = this._iConfiguration.GetSection("ConnectionStrings").GetSection("Read").GetChildren().Select(s => s.Value).ToArray();

            Console.WriteLine($"AllowedHosts={AllowedHosts} today={today} writeConn={writeConn} readConn0={readConn0} _SqlConnectionStringRead={string.Join(",", _SqlConnectionStringRead)}");

            return View();
        }

        /// <summary>
        /// dotnet run --urls="http://*:5726" ip="127.0.0.1" /port=5726 ConnectionStrings:Write=CommandLineArgument
        /// </summary>
        /// <returns></returns>
        public IActionResult CommandLine()
        {
            /*
             var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddCommandLine(args)
            .Build();
             */
            string urls = this._iConfiguration["urls"];
            string ip = this._iConfiguration["ip"];
            string port = this._iConfiguration["port"];
            string writeConn = this._iConfiguration["ConnectionStrings:Write"];

            Console.WriteLine($"urls={urls} ip={ip} port={port} writeConn={writeConn} ");

            return View();
        }

        public IActionResult Bind()
        {
            //bind
            RabbitMQOptions rabbitMQOptions1 = new RabbitMQOptions();
            this._iConfiguration.GetSection("RabbitMQOptions").Bind(rabbitMQOptions1);
            Console.WriteLine($"HostName={rabbitMQOptions1.HostName}");

            //Get
            RabbitMQOptions rabbitMQOptions2 = this._iConfiguration.GetSection("RabbitMQOptions").Get<RabbitMQOptions>();
            Console.WriteLine($"HostName2={rabbitMQOptions2.HostName}");

            return View();
        }

        public class RabbitMQOptions
        {
            public string HostName { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public IActionResult Memory()
        {
            string HostName = this._iConfiguration["RabbitMQOptions:HostName"];
            string TodayMemory = this._iConfiguration["TodayMemory"];
            Console.WriteLine($"HostName={HostName}");
            Console.WriteLine($"TodayMemory={TodayMemory}");

            RabbitMQOptions rabbitMQOptions1 = new RabbitMQOptions();
            this._iConfiguration.GetSection("RabbitMQOptions").Bind(rabbitMQOptions1);
            Console.WriteLine($"HostName={rabbitMQOptions1.HostName}");

            RabbitMQOptions rabbitMQOptions2 = this._iConfiguration.GetSection("RabbitMQOptions").Get<RabbitMQOptions>();
            Console.WriteLine($"HostName2={rabbitMQOptions2.HostName}");

            return View();
        }


        public IActionResult XML()
        {
            string HostName = this._iConfiguration["RabbitMQOptions:HostName"];
            string TodayXML = this._iConfiguration["TodayXML"];
            Console.WriteLine($"HostName={HostName}");
            Console.WriteLine($"TodayXML={TodayXML}");

            RabbitMQOptions rabbitMQOptions1 = new RabbitMQOptions();
            this._iConfiguration.GetSection("RabbitMQOptions").Bind(rabbitMQOptions1);
            Console.WriteLine($"HostName={rabbitMQOptions1.HostName}");

            RabbitMQOptions rabbitMQOptions2 = this._iConfiguration.GetSection("RabbitMQOptions").Get<RabbitMQOptions>();
            Console.WriteLine($"HostName2={rabbitMQOptions2.HostName}");

            return View();
        }


        public IActionResult Custom()
        {
            string HostName = this._iConfiguration["RabbitMQOptions-Custom:HostName"];
            //this._iConfiguration["RabbitMQOptions-Custom:HostName"] = "1233";
            string TodayCustom = this._iConfiguration["TodayCustom"];
            Console.WriteLine($"HostName={HostName}");
            Console.WriteLine($"TodayXML={TodayCustom}");

            RabbitMQOptions rabbitMQOptions1 = new RabbitMQOptions();
            this._iConfiguration.GetSection("RabbitMQOptions-Custom").Bind(rabbitMQOptions1);
            Console.WriteLine($"HostName={rabbitMQOptions1.HostName}");

            RabbitMQOptions rabbitMQOptions2 = this._iConfiguration.GetSection("RabbitMQOptions-Custom").Get<RabbitMQOptions>();
            Console.WriteLine($"HostName2={rabbitMQOptions2.HostName}");

            return View();
        }
    }

}
