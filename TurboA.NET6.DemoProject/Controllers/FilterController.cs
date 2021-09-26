using Castle.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TurboA.AgileFramework.WebCore.AuthorizationExtend;
using TurboA.AgileFramework.WebCore.FilterExtend;
using TurboA.AgileFramework.WebCore.FilterExtend.SimpleExtend;

namespace TurboA.NET6.DemoProject.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    //[CustomControllerOrderFilterAttribute]
    //[CustomControllerOrderFilterAttribute(Order = 20)]//对控制器里面所有的Action生效
    public class FilterController : Controller, IActionFilter, IAsyncActionFilter//, IResultFilter, IAsyncResultFilter
    {
        #region Identity
        private readonly IConfiguration _iConfiguration = null;
        private readonly ILogger<FilterController> _logger;

        public FilterController(IConfiguration configuration
            , ILogger<FilterController> logger)
        {
            this._iConfiguration = configuration;
            this._logger = logger;
        }
        #endregion

        #region Index+ControllerFilter
        /// <summary>
        /// dotnet run --urls="http://*:5726" ip="127.0.0.1" /port=5726 ConnectionStrings:Write=CommandLineArgument
        /// http://localhost:5726/Filter/Index
        /// 
        /// IActionFilter和IAsyncActionFilter默认已实现，可以override
        /// IResultFilter和IAsyncResultFilter没有默认实现，需要实现
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            this._logger.LogWarning($"This is {nameof(FilterController)}-Index LogWarning");

            return View();
        }

        #region 控制器继承Filter
        ///// <summary>
        ///// IActionFilter和IAsyncActionFilter默认已实现，可以override
        ///// </summary>
        ///// <param name="context"></param>
        //public override void OnActionExecuted(ActionExecutedContext context)
        //{
        //    Console.WriteLine($"This {nameof(FilterController)} OnActionExecuted");
        //}
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    Console.WriteLine($"This {nameof(FilterController)} OnActionExecuting");
        //}
        //public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    Console.WriteLine($"This {nameof(FilterController)} OnActionExecutionAsync  --Begin");
        //    await next.Invoke();
        //    Console.WriteLine($"This {nameof(FilterController)} OnActionExecutionAsync  --End");
        //}
        ///// <summary>
        ///// IResultFilter和IAsyncResultFilter没有默认实现，需要实现
        ///// </summary>
        ///// <param name="context"></param>
        //public void OnResultExecuting(ResultExecutingContext context)
        //{
        //    Console.WriteLine($"This {nameof(CustomShowResultFilterAttribute)} OnResourceExecuting ");
        //}
        //public void OnResultExecuted(ResultExecutedContext context)
        //{
        //    Console.WriteLine($"This {nameof(CustomShowResultFilterAttribute)} OnResourceExecuted ");
        //}
        //public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        //{
        //    Console.WriteLine($"This {nameof(FilterController)} OnResultExecutionAsync  --Begin");
        //    await next.Invoke();
        //    Console.WriteLine($"This {nameof(FilterController)} OnResultExecutionAsync --End");
        //}

        #endregion

        #endregion

        #region Filter排序
        /// <summary>
        /// http://localhost:5726/Filter/Info
        /// 1 全局---控制器---Action  
        /// 2 Order默认0，从小到大执行  可以负数
        /// </summary>
        /// <returns></returns>
        //[TypeFilter(typeof(CustomActionOrderFilterAttribute), Order = 10, IsReusable = false)]
        //[CustomActionOrderFilterAttribute(Order = 10)]
        //[CustomActionOrderFilterAttribute(Remark = "Before")]
        //[CustomActionOrderFilterAttribute(Remark = "After")]//我后执行
        //[CustomActionCacheFilterAttribute(Order = -1)]
        //[IResourceFilter]
        public IActionResult Info()
        {
            this._logger.LogWarning($"This is {nameof(FilterController)}-Info LogWarning");

            base.ViewBag.Now = DateTime.Now;
            Thread.Sleep(2000);
            return View();
        }
        #endregion

        #region Filter的IOC注入问题
        /// <summary>
        /// http://localhost:5726/Filter/ExceptionIOC
        /// 
        /// </summary>
        /// <returns></returns>
        //[ServiceFilter(typeof(CustomExceptionFilterAttribute))]//需要IOC注册
        //[TypeFilter(typeof(CustomExceptionFilterAttribute))]
        //[CustomIOCFilterFactory(typeof(CustomExceptionFilterAttribute))]
        //[CustomAttribute(new LoggerFactory().CreateLogger<CustomAttribute>())]
        //[CustomAttribute(CustomAttribute.Name)]
        public IActionResult ExceptionIOC()
        {
            this._logger.LogWarning($"This is {nameof(FilterController)}-ExceptionIOC LogWarning");

            base.ViewBag.Now = DateTime.Now;
            Thread.Sleep(2000);

            throw new Exception("This is Eleven's ExceptionIOC");

            return View();
        }
        #endregion

        #region Authorization
        /// <summary>
        /// http://localhost:5726/Filter/Authorization
        /// http://localhost:5726/Filter/Authorization?UserName=Eleven
        /// </summary>
        /// <returns></returns>
        [CustomAuthorizationFilter]
        [CustomAsyncAuthorizationFilter]
        //[AllowAnonymous]
        [CustomShowAlwaysRunResultFilterAttribute(Remark = "Authorization-Always")]
        [CustomAsyncShowAlwaysRunResultFilterAttribute(Remark = "Authorization-AsyncAlways")]
        //[CustomShowResultFilterAttribute(Remark = "Authorization-CommonResult")]
        //[CustomAsyncShowResultFilterAttribute(Remark = "Authorization-AsyncCommonResult")]
        public async Task<IActionResult> Authorization()
        {
            await Task.CompletedTask;
            this._logger.LogWarning($"This is {nameof(FilterController)}-Authorization LogWarning");

            base.ViewBag.Now = DateTime.Now;
            Thread.Sleep(2000);

            return View();

            //try
            //{
            //    1 / 0;
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

        }

        [CustomShowAlwaysRunResultFilterAttribute(Remark = "Login-Always")]
        [CustomAsyncShowAlwaysRunResultFilterAttribute(Remark = "Login-AsyncAlways")]
        //[CustomShowResultFilterAttribute(Remark = "Login-CommonResult")]
        //[CustomAsyncShowResultFilterAttribute(Remark = "Login-AsyncCommonResult")]
        public async Task<IActionResult> Login()
        {
            await Task.CompletedTask;
            return new JsonResult(new
            {
                Result = true,
                Message = "访问登陆页"
            });
        }
        #endregion

        #region Resource
        /// <summary>
        /// http://localhost:5726/Filter/Resource
        /// 
        /// </summary>
        /// <returns></returns>
        [CustomRedisResourceFilter]//外面套一层ResourceFilter
        [CustomDictionaryResourceFilter]
        //[CustomAsyncDictionaryResourceFilter]
        [CustomShowAlwaysRunResultFilterAttribute(Remark = "Resource-Always")]
        [CustomAsyncShowAlwaysRunResultFilterAttribute(Remark = "Resource-AsyncAlways")]
        [CustomShowResultFilterAttribute(Remark = "Resource-CommonResult")]
        [CustomAsyncShowResultFilterAttribute(Remark = "Resource-AsyncCommonResult")]
        [CustomIOCFilterFactory(typeof(CustomExceptionFilterAttribute))]
        public async Task<IActionResult> Resource()
        {
            await Task.CompletedTask;
            this._logger.LogWarning($"This is {nameof(FilterController)}-Resource LogWarning");

            base.ViewBag.Now = DateTime.Now;
            Thread.Sleep(2000);

            return View();
        }
        #endregion

        #region Exception
        /// <summary>
        /// http://localhost:5726/Filter/Exception
        /// 
        /// 加上全局的带注入ExceptionFilter
        /// </summary>
        /// <returns></returns>
        [CustomAsyncExceptionFilter]
        [CustomSyncExceptionFilter]
        [CustomShowAlwaysRunResultFilterAttribute(Remark = "Exception-Always")]
        [CustomAsyncShowAlwaysRunResultFilterAttribute(Remark = "Exception-AsyncAlways")]
        [CustomShowResultFilterAttribute(Remark = "Exception-CommonResult")]
        [CustomAsyncShowResultFilterAttribute(Remark = "Exception-AsyncCommonResult")]
        public IActionResult Exception()
        {
            this._logger.LogWarning($"This is {nameof(FilterController)}-Exception LogWarning");

            base.ViewBag.Now = DateTime.Now;
            Thread.Sleep(2000);

            throw new Exception("This is Eleven's Exception");

            return View();
        }


        #region Test
        public IActionResult ExceptionCatch()
        {
            this._logger.LogWarning($"This is {nameof(FilterController)}-ExceptionCatch LogWarning");
            try
            {
                throw new Exception("ExceptionCatch");//也可以是调用类库方法出了异常
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new JsonResult(new
            {
                Result = true,
                Message = $"访问{nameof(FilterController)}-ExceptionCatch成功"
            });
        }
        public ActionResult ExceptionUnCatch()
        {
            this._logger.LogWarning($"This is {nameof(FilterController)}-ExceptionUnCatch LogWarning");
            throw new Exception("ExceptionCatch");
            return new JsonResult(new
            {
                Result = true,
                Message = $"访问{nameof(FilterController)}-ExceptionCatch成功"
            });
        }


        public ActionResult ExceptionView()
        {
            this._logger.LogWarning($"This is {nameof(FilterController)}-ExceptionView LogWarning");
            return View();
        }

        //View异常
        //ResultFilter异常
        //Action异常
        //ActionFilter异常
        //控制器实例化异常
        //ExceptionFilter异常
        //ResourceFilter异常
        //AuthorizationFilter异常
        //中间件异常
        //AlwaysRunResultFilter异常
        #endregion

        #endregion

        #region Action
        /// <summary>
        /// http://localhost:5726/Filter/Action
        /// http://localhost:5726/Filter/Action/1
        /// </summary>
        /// <returns></returns>
        [CustomShowActionFilter(Order = 4)]
        [CustomAsyncShowActionFilter(Order = 5)]
        //[CustomShowAlwaysRunResultFilterAttribute(Remark = "Action-Always")]
        //[CustomAsyncShowAlwaysRunResultFilterAttribute(Remark = "Action-AsyncAlways")]
        [CustomShowResultFilterAttribute(Remark = "Action-CommonResult")]
        //[CustomAsyncShowResultFilterAttribute(Remark = "Action-AsyncCommonResult")]
        [TypeFilter(typeof(CustomExceptionFilterAttribute))]
        //[CustomSyncExceptionFilterAttribute]//异常Filter抛异常
        public async Task<IActionResult> Action(int? id)
        {
            await Task.CompletedTask;
            this._logger.LogWarning($"This is {nameof(FilterController)}-Action LogWarning");

            base.ViewBag.Now = DateTime.Now;
            Thread.Sleep(2000);

            if (id == 1)
            {
                throw new Exception("This is Eleven's Filter/Action/1  Exception");
            }

            return View();
        }
        #endregion

        #region Result+AlwaysRunResult
        /// <summary>
        /// http://localhost:5726/Filter/Result
        /// http://localhost:5726/Filter/Result/1
        /// </summary>
        /// <returns></returns>
        [CustomShowResultFilterAttribute(Remark = "Result-CommonResult", Order = 2)]
        [CustomAsyncShowResultFilterAttribute(Remark = "Result-AsyncCommonResult", Order = 1)]
        [CustomShowAlwaysRunResultFilterAttribute(Remark = "Result-Always", Order = 4)]
        //[CustomAsyncShowAlwaysRunResultFilterAttribute(Remark = "Result-AsyncAlways")]
        //[TypeFilter(typeof(CustomExceptionFilterAttribute))]
        //[CustomAsyncExceptionFilterAttribute]
        public IActionResult Result(int? id)
        {
            this._logger.LogWarning($"This is {nameof(FilterController)}-Result LogWarning");

            base.ViewBag.Now = DateTime.Now;
            Thread.Sleep(2000);

            base.ViewBag.Id = id;//去View里面抛异常

            return View();
        }

        /// <summary>
        /// http://localhost:5726/Filter/AlwaysRunResult
        /// http://localhost:5726/Filter/AlwaysRunResult/123
        /// </summary>
        /// <returns></returns>
        [CustomShowResultFilterAttribute(Remark = "AlwaysRunResult-CommonResult")]
        [CustomAsyncShowResultFilterAttribute(Remark = "AlwaysRunResult-AsyncCommonResult")]
        [CustomShowAlwaysRunResultFilterAttribute(Remark = "AlwaysRunResult-Always")]
        [CustomAsyncShowAlwaysRunResultFilterAttribute(Remark = "AlwaysRunResult-AsyncAlways")]
        public IActionResult AlwaysRunResult(int? id)
        {
            this._logger.LogWarning($"This is {nameof(FilterController)}-AlwaysRunResult LogWarning");

            base.ViewBag.Now = DateTime.Now;
            Thread.Sleep(2000);

            if (id is null)
                return new RedirectResult("~/Filter/AlwaysRunResult"); ;

            return View();
        }
        #endregion
    }
}
