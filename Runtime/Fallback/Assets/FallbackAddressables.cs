using MirraGames.SDK.Common;
using System;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IAddressables))]
    public class FallbackAddressables : CommonAddressables {

        protected override void LoadAddressableImpl<T>(string addressableKey, Action<T> onSuccess, Action onError = null) {
            Logger.NotImplementedWarning(this, nameof(LoadAddressableImpl));
            onError?.Invoke();
        }

        protected override void ReleaseAddressableImpl(string addressableKey) {
            Logger.NotImplementedWarning(this, nameof(ReleaseAddressableImpl));
        }

    }

}