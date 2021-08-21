using System;
using System.Collections.Generic;
using System.Text;

namespace TurboA.AgileFramework.Pandora.CustomContainer
{
    public interface ITurboAContainer
    {
        void Register<TFrom, TTo>(string shortName = null, object[] paraList = null, LifetimeType lifetimeType = LifetimeType.Transient) where TTo : TFrom;

        void RegisterType(Type typeFrom, Type typeTo, LifetimeType lifetimeType = LifetimeType.Transient);

        TFrom Resolve<TFrom>(string shortName = null);

        object Resolve(Type type);

        ITurboAContainer CreateChildContainer();
    }
}
