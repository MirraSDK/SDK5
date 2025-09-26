using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using MirraGames.SDK.Common;

namespace MirraGames.SDK.UnityEngine {

    [Provider(typeof(IAddressables))]
    public class UnityEngineAddressables : CommonAddressables {

        private readonly Dictionary<string, AsyncOperationHandle> loadedHandles = new();

        protected override void LoadAddressableImpl<T>(string addressableKey, Action<T> onSuccess, Action onError = null) {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(addressableKey);
            loadedHandles[addressableKey] = handle;
            handle.Completed += (operation) => {
                if (operation.Status == AsyncOperationStatus.Succeeded) {
                    onSuccess?.Invoke(operation.Result);
                }
                else {
                    loadedHandles.Remove(addressableKey);
                    onError?.Invoke();
                }
            };
        }

        protected override void ReleaseAddressableImpl(string addressableKey) {
            if (loadedHandles.TryGetValue(addressableKey, out AsyncOperationHandle handle)) {
                Addressables.Release(handle);
                loadedHandles.Remove(addressableKey);
            }
        }

    }

}