using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboA.AgileFramework.WebCore.MiddlewareExtend
{
    /// <summary>
    /// 修改Stream
    /// http://localhost:5000/api/webapi/post
    /// Postman--Post---Body有个Name=TurboA
    /// </summary>
    public class StreamWriteMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<StreamWriteMiddleware> _logger;

        public StreamWriteMiddleware(RequestDelegate next, ILogger<StreamWriteMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            this._logger.LogWarning($"StreamWriteMiddleware Handle Request: " + context.Request.Path);

            context.Request.EnableBuffering();
            var requestStream = context.Request.Body;
            var responseStream = context.Response.Body;
            try
            {
                using (var newRequest = new MemoryStream())
                {
                    context.Request.Body = newRequest; //替换request流
                    using (var newResponse = new MemoryStream())
                    {
                        context.Response.Body = newResponse;//替换response流
                        using (var reader = new StreamReader(requestStream))
                        {
                            string requestBody = await reader.ReadToEndAsync();//读取原始请求流的内容
                            if (string.IsNullOrEmpty(requestBody))//为空直接走下一环节
                            {
                                await _next.Invoke(context);
                            }
                            else
                            {
                                using (var writer = new StreamWriter(newRequest))
                                {
                                    await writer.WriteAsync(requestBody.ToUpper());//直接改成大写
                                    await writer.FlushAsync();
                                    newRequest.Position = 0;
                                    context.Request.Body = newRequest;
                                    await _next(context);
                                }
                            }
                        }

                        //获取和修改响应
                        string responseBody = null;
                        using (var reader = new StreamReader(newResponse))
                        {
                            newResponse.Position = 0;
                            responseBody = await reader.ReadToEndAsync();
                        }
                        using (var writer = new StreamWriter(responseStream))
                        {
                            await writer.WriteAsync(responseBody.ToLower());//响应全部小写
                            await writer.FlushAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError($" http中间件发生错误: " + ex.ToString());
            }
            finally
            {
                context.Request.Body = requestStream;
                context.Response.Body = responseStream;
            }
        }
    }
}
