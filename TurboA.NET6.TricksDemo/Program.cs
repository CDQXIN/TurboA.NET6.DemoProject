using System;
using System.Diagnostics.Contracts;
using TurboA.AgileFramework.Pandora.CustomAOP;
using TurboA.AgileFramework.Pandora.CustomContainer;
using TurboA.NET6.TricksDemo.ActionInit;
using TurboA.NET6.TricksDemo.BuilderShow;
using TurboA.NET6.TricksDemo.CustomeAOP;
using TurboA.NET6.TricksInterface;
using TurboA.NET6.TricksService;

namespace TurboA.NET6.TricksDemo
{
    /// <summary>
    /// 展示核心套路
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello World!");

                #region 建造者Builder pattern
                //{
                //    Computer computer1 = new Computer("cpu", "ram");
                //    Computer computer2 = new Computer("cpu", "ram", 2);
                //    Computer computer3 = new Computer("cpu", "ram", 2, "罗技");
                //}
                //{
                //    Computer computer1 = new Computer("cpu", "ram");
                //    computer1.SetUSBCount(2);
                //    computer1.SetKeyboard("1");
                //    computer1.SetDisplay("233");
                //}
                //{
                //    ComputerBuilder computerBuilder = new ComputerBuilder("cpu", "ram")
                //        .SetUSBCount(2)
                //        .SetKeyboard("1")
                //        .SetDisplay("233")
                //        ;
                //    Computer computer1 = computerBuilder.Build();
                //}
                #endregion

                #region  委托初始化
                ////委托：是个类class--里面包裹方法，也就是可以传递方法，动作--委托声明时没有执行的，调用时才执行的

                ////准备new一个对象，并且指定一系列参数，要么构造函数，要么初始化方法，当然还有默认值， 如果经常变动，经常扩展呢？Action传递
                ////1  传值，换成传递对象-----增加属性不影响
                ////2  不关注对象如何初始化，甚至前后加逻辑---转移职责(Extend完成)
                ////3  通过委托直接操作Option，不关注Option的初始化---转移职责(Extend完成)
                ////4  还能完成方法初始化
                ////只有这样，组件的扩展变化，才不影响调用者
                //{
                //    //一般是第三方的类库---传递多个参数---不如做成传递实体对象---如果还有方法需要调用呢
                //    KestrelHost kestrelHost = new KestrelHost(true, false);
                //    kestrelHost.EnableAltSvc = false;
                //    kestrelHost.Init1();
                //    kestrelHost.Init2();
                //    kestrelHost.Init3();
                //    //才算准备好----调用方得知道很多细节
                //}
                //{
                //    KestrelHostExtend.BuildAndConfigureKestrelHost(
                //        option =>
                //    {
                //        option.AddServerHeader = true;
                //        option.Init1();
                //        option.Init2();
                //        option.Init3();
                //    });
                //}
                #endregion

                #region 委托分派
                {
                    ////解决循环引用
                    //IUserService userService = new UserService();
                    //userService.Login("TurboA", "123456");
                }
                #endregion

                #region 委托组装流程
                //自定义容器的AOP扩展

                //抓住一个点，就能无限制拓展--看懂了源码不代表能看懂应用，中间件就是案例
                {
                    //CustomAOPTest.Show();
                    //Castle通过Emit技术去动态生成了一个代理类，这个类是目标类的子类CommonClass，然后就可以植入前后逻辑，就可以AOP了--有个切口了
                    //可能有很多需求，缓存、前后日志、时间统计、异常处理---都应该写在CustomInterceptor里面
                    //但是，假如我需要的是，能够随意增减AOP逻辑，或者就叫定制逻辑----这种可以靠特性+反射---invocation.Method.GetCustomAttributes() is null  有就执行逻辑  没有就算了
                    //细思恐极---逻辑跟逻辑是不一样的----前日志-后日志 都挺简单----性能监控，同时要求前后、异常处理也是同时要求前后---而且还有顺序调换需求
                    //这里就又诞生了一个新的需求------流程组装-----就是刚才用特性是判断后执行逻辑，现在就是用特性判断后，只是组装流程，但是不执行
                    //其核心就是委托---靠委托来组装流程---然后执行时就可以按顺序来

                    ITurboAContainer container = new TurboAContainer();
                    container.Register<ITricksServiceA, TricksServiceA>();
                    var serviceA = container.Resolve<ITricksServiceA>();
                    serviceA.Show();
                    serviceA.Show1();
                }
                #endregion
            }
            catch (Exception ex)
            {

            }
            Console.ReadLine();
        }
    }
}
