using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboA.AgileFramework.WebCore.StartupExtend
{
    /// <summary>
    /// 发生在Run的时候，执行Startup类的Configure方法之前
    /// 
    /// 需要注册到IOC容器去
    /// 
    /// 头尾拦截加东西----缓存初始化、配置文件检测提前报错、黑白名单组件
    /// </summary>
    public class CustomStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return  new Action<IApplicationBuilder>(
             app =>
            {
                app.Use(next =>
                {
                    Console.WriteLine($"This is {nameof(CustomStartupFilter)} middleware 1");
                    return new RequestDelegate(
                        async context =>
                        {
                            await context.Response.WriteAsync($"This is {nameof(CustomStartupFilter)} Hello World 1 start");
                            await next.Invoke(context);
                            await context.Response.WriteAsync($"This is {nameof(CustomStartupFilter)} Hello World 1   end");
                            await Task.Run(() => Console.WriteLine($"{nameof(CustomStartupFilter)} 12345678797989"));
                        });
                });
                next.Invoke(app);//不next 后面就没法配置
            }
            );
        }
    }
}
