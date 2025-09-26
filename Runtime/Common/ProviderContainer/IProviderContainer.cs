using System;

namespace MirraGames.SDK.Common {

    public interface IProviderContainer {

        bool Contains<T>();
        void Register<T>(T instance, Enum provider = null);
        void Register(Type type, object instance, Enum provider = null);
        T First<T>(T defaultValue = default);
        T Find<T>(Enum provider, T defaultValue = default);

    }

}