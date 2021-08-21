using NConsul.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboA.AgileFramework.Core.ConsulExtend
{
    public interface IConsulDistributed
    {
        void KVShow();
        Task<IDistributedLock> AcquireLock(string key);
        Task ExecuteLocked(string key, Action action);
    }
}
