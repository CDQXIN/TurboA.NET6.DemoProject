using System;

namespace Zhaoxi.NET6.TricksDemo.CustomeAOP
{
    public class TricksServiceA : ITricksServiceA
    {
        public TricksServiceA()
        {
            Console.WriteLine($"{this.GetType().Name}被构造。。。");
        }

        public void Show()
        {
            Console.WriteLine("A123456");
        }

        public void Show1()
        {
            Console.WriteLine("A123456111111111111111");
        }
    }
}
