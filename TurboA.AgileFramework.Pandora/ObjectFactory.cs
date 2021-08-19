using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
//using Zhaoxi.IOCDI.IBLL;
//using Zhaoxi.IOCDI.IDAL;

namespace Zhaoxi.AgileFramework.Pandora
{
    public class ObjectFactory
    {
        //public static IUserDAL CreateDAL()
        //{
        //    IUserDAL userDAL = null;
        //    //不能依赖细节 但是又要创建对象

        //    string config = ConfigurationManager.GetNode("IUserDAL");

        //    //Assembly assembly = Assembly.LoadFile(config.Split(',')[1]);
        //    //Type type = assembly.GetType(config.Split(',')[0]);

        //    Assembly assembly = Assembly.Load(config.Split(';')[1]);
        //    //load需要dev.json配置依赖关系 
        //    //LoadFile 完整路径    LoadFromUnsafe 直接dll名称
        //    Type type = assembly.GetType(config.Split(';')[0]);

        //    userDAL = (IUserDAL)Activator.CreateInstance(type);
        //    return userDAL;
        //}
        ////如果事先没有引用dll而是copydll，load失败了

        //public static IUserBLL CreateBLL(IUserDAL userDAL)
        //{
        //    IUserBLL userBLL = null;
        //    //不能依赖细节 但是又要创建对象

        //    string config = ConfigurationManager.GetNode("IUserBLL");

        //    Assembly assembly = Assembly.Load(config.Split(',')[1]);
        //    Type type = assembly.GetType(config.Split(',')[0]);
        //    userBLL = (IUserBLL)Activator.CreateInstance(type, new object[] { userDAL });
        //    return userBLL;
        //}

    }
}
