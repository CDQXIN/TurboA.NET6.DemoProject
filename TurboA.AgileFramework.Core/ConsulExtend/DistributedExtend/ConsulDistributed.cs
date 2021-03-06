using NConsul;
using NConsul.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboA.AgileFramework.Core.ConsulExtend
{
    public class ConsulDistributed : IConsulDistributed
    {
        #region Identity
        /// <summary>
        /// 
        /// </summary>
        private string _Address = $"http://127.0.0.1:{8500}/";
        private string _Datacenter = "dc1";
        private static string prefix = "consullock_";  // 同步锁参数前缀
        private ConsulClient consulClient;

        public ConsulDistributed() : this($"http://127.0.0.1:{8500}/")
        {
        }
        public ConsulDistributed(string address)
        {
            this._Address = address;
            this.consulClient = new ConsulClient(c =>
            {
                c.Address = new Uri(this._Address);
                c.Datacenter = this._Datacenter;
            });
        }
        #endregion
        public void KVShow()
        {
            using (ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri(this._Address);
                c.Datacenter = this._Datacenter;
            }))
            {
                client.KV.Put(new KVPair("TurboA") { Value = Encoding.UTF8.GetBytes("This is Teacher") });
                Console.WriteLine(client.KV.Get("TurboA"));
                client.KV.Delete("TurboA");
            }
        }

        #region 分布式锁

        /// <summary>
        /// 需要先初始化
        /// </summary>
        /// <param name="key"></param>
        public Task<IDistributedLock> AcquireLock(string key)
        {
            LockOptions opts = new LockOptions($"{prefix}{key}");//默认值
            //{
            //    LockRetryTime = TimeSpan.FromSeconds(5),
            //    LockWaitTime = TimeSpan.FromSeconds(3),
            //    MonitorRetryTime = TimeSpan.FromSeconds(1)
            //};
            return this.consulClient.AcquireLock(opts);
        }



        /// <summary>
        /// 包装了一层，委托嵌套
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Task ExecuteLocked(string key, Action action)
        {
            //Console.WriteLine($"{prefix}{key}");
            LockOptions opts = new LockOptions($"{prefix}{key}");//默认值
            //{
            //    LockRetryTime = TimeSpan.FromSeconds(5),
            //    LockWaitTime = TimeSpan.FromSeconds(3),
            //    MonitorRetryTime = TimeSpan.FromSeconds(1)
            //};
            return this.consulClient.ExecuteLocked(opts, action);
        }

        public void Dispose()
        {
            if (this.consulClient != null)
            {
                this.consulClient.Dispose();
            }
        }
        #endregion
    }
}
