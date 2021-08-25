using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TurboA.NET6.DemoProject.Controllers
{
    public class RouteController : Controller
    {
        private readonly IConfiguration _iConfiguration = null;
        private readonly ILogger<RouteController> _logger;

        public RouteController(IConfiguration configuration
            , ILogger<RouteController> logger)
        {
            this._iConfiguration = configuration;
            this._logger = logger;
        }

        /// <summary>
        /// dotnet run --urls="http://*:5726" ip="127.0.0.1" /port=5726 ConnectionStrings:Write=CommandLineArgument
        /// http://localhost:5726/en/route/index
        /// http://localhost:5726/ch/route1/index1
        /// http://localhost:5726/hk/route2/index2
        /// </summary>
        /// <returns></returns>

        public IActionResult Index()
        {
            this._logger.LogWarning("This is RouteController-Index LogWarning");
            string des = $"language={this.HttpContext.Request.RouteValues["language"]}&controller={this.HttpContext.Request.RouteValues["controller"]}&action={this.HttpContext.Request.RouteValues["action"]}";
            Console.WriteLine(des);
            base.ViewBag.Des = des;
            return View();
        }

        /// <summary>
        /// http://localhost:5726/Item/133.html
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("/Item/{id:int}.html")]
        public IActionResult PageInfo(int id)
        {
            this._logger.LogWarning("This is RouteController-PageInfo LogWarning");
            string des = $"controller={this.HttpContext.Request.RouteValues["controller"]}&action={this.HttpContext.Request.RouteValues["action"]}&Id={this.HttpContext.Request.RouteValues["id"]}";
            base.ViewBag.Des = des;
            return View();
        }

        /// <summary>
        /// http://localhost:5726/Route/Data/2019-11
        /// http://localhost:5726/Route/Data/2019-13
        /// http://localhost:5726/Route/Data?year=2019&month=11
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Data(int year, int month)
        {
            this._logger.LogWarning("This is RouteController-Data LogWarning");
            string des = $"controller={this.HttpContext.Request.RouteValues["controller"]}&action={this.HttpContext.Request.RouteValues["action"]}&路由Id={this.HttpContext.Request.RouteValues["id"]}&路由year={this.HttpContext.Request.RouteValues["year"]}&路由month={this.HttpContext.Request.RouteValues["month"]}&参数year={year}&参数month={month}";
            base.ViewBag.Des = des;
            return View();
        }
    }
}
