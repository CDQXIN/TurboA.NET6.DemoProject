using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TurboA.NET6.DemoProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebAPIController : ControllerBase
    {
        [HttpPost]
        [Route("Post")]
        public IActionResult Post()
        {
            Console.WriteLine(base.HttpContext.Request.Form["Name"]);

            return new JsonResult(new
            {
                Name = base.HttpContext.Request.Form["Name"],
                Result = 1,
                Message = $"This is {nameof(WebAPIController)} {nameof(Post)}"
            });
        }
    }
}
