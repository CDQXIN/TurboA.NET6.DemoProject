using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboA.AgileFramework.WebCore.RouteExtend
{
    //    public static class CustomRouteExtensions
    //    {
    //        #region DynamicRoute  
    //        /// <summary>
    //        /// 需要提供数据获取
    //        /// </summary>
    //        /// <param name="services"></param>
    //        public static void AddDynamicRoute(this IServiceCollection services)
    //        {
    //            services.AddSingleton<TranslationTransformer>();
    //            services.AddSingleton<TranslationDatabase>();
    //        }
    //        /// <summary>
    //        /// 需要配置路由
    //        /// </summary>
    //        /// <param name="endpoints"></param>
    //        public static void UseDynamicRouteDefault(this IEndpointRouteBuilder endpoints)
    //        {
    //            endpoints.MapDynamicControllerRoute<TranslationTransformer>("{language}/{controller}/{action}");
    //        }
    //        #endregion

    //        #region MapGet
    //        public static void UseMapGetDefault(this IEndpointRouteBuilder endpoints)
    //        {
    //            endpoints.MapGet("/hello/{name:alpha}", async context =>
    //            {
    //                var name = context.Request.RouteValues["name"];
    //                await context.Response.WriteAsync($"Hello {name}!");
    //            }); //处理动作
    //        }
    //        #endregion

    //        #region Map其他信息

    //        #endregion
    //    }
}
