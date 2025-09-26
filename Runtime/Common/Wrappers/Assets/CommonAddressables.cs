using System;

namespace MirraGames.SDK.Common {

    [Wrapper]
    public abstract partial class CommonAddressables : IAddressables {

        protected abstract void LoadAddressableImpl<T>(string addressableKey, Action<T> onSuccess, Action onError = null);
        protected abstract void ReleaseAddressableImpl(string addressableKey);

        public void LoadAddressable<T>(string addressableKey, Action<T> onSuccess, Action onError = null) {
            Logger.CreateText(this, nameof(LoadAddressable), addressableKey);
            try {
                LoadAddressableImpl(addressableKey, onSuccess, onError);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(LoadAddressable), exception);
            }
        }

        public void ReleaseAddressable(string addressableKey) {
            Logger.CreateText(this, nameof(ReleaseAddressable), addressableKey);
            try {
                ReleaseAddressableImpl(addressableKey);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(ReleaseAddressable), exception);
            }
        }

    }

}