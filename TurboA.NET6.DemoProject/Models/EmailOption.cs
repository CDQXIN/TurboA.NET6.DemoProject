using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TurboA.NET6.DemoProject.Models
{
    /// <summary>
    /// 测试Option  必须是无参数构造函数
    /// </summary>
    public class EmailOption
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public string From { get; set; }
    }
}
