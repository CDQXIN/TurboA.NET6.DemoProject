using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhaoxi.NET6.DemoProject.Models;

namespace Zhaoxi.NET6.DemoProject.Controllers
{
    public class OptionController : Controller
    {
        private readonly IConfiguration _iConfiguration = null;
        private readonly ILogger<OptionController> _logger;

        private IOptions<EmailOption> _optionsDefault;//直接单例，读出来就缓存，不支持数据变化，性能高--只能度默认名字
        private IOptionsMonitor<EmailOption> _optionsMonitor;//只读一次，写入缓存-----但是支持数据修改，靠的是监听文件更新(onchange)数据，实时变更
        private IOptionsSnapshot<EmailOption> _optionsSnapshot;//作用域注册，一次请求内数据是缓存不变的，但是不同请求是每次都会重新第一次数据

        public OptionController(IOptions<EmailOption> options
            , IOptionsMonitor<EmailOption> optionsMonitor
            , IOptionsSnapshot<EmailOption> optionsSnapshot
            , IConfiguration configuration
            , ILogger<OptionController> logger)
        {
            this._optionsDefault = options;
            this._optionsMonitor = optionsMonitor;
            this._optionsSnapshot = optionsSnapshot;

            this._iConfiguration = configuration;
            this._logger = logger;
        }

        /// <summary>
        /// http://localhost:5726/Option
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            this._logger.LogWarning($"This is {nameof(OptionController)} Index");

            //可以直接访问配置文件---但是使用的地方得知道配置文件路径
            //就是IOC注入参数--很僵化
            base.ViewBag.defaultEmailOption = this._optionsDefault.Value;

            base.ViewBag.defaultEmailOption1 = _optionsMonitor.CurrentValue;//_optionsMonitor.Get(Microsoft.Extensions.Options.Options.DefaultName);
            base.ViewBag.fromMemoryEmailOption1 = _optionsMonitor.Get("FromMemory");
            base.ViewBag.fromConfigurationEmailOption1 = _optionsMonitor.Get("FromConfiguration");
            base.ViewBag.fromConfigurationEmailOptionNew = _optionsMonitor.Get("FromConfigurationNew");//直接目录修改配置文件，就能看到变化

            base.ViewBag.defaultEmailOption2 = _optionsSnapshot.Value;//_optionsSnapshot.Get(Microsoft.Extensions.Options.Options.DefaultName);
            base.ViewBag.fromMemoryEmailOption2 = _optionsSnapshot.Get("FromMemory");
            base.ViewBag.fromMemoryEmailOption2 = _optionsSnapshot.Get("FromMemory");
            base.ViewBag.fromMemoryEmailOption2 = _optionsSnapshot.Get("FromMemory");//3遍都是一样的
            base.ViewBag.fromConfigurationEmailOption2 = _optionsSnapshot.Get("FromConfiguration");

            return View();
        }

    }
}
