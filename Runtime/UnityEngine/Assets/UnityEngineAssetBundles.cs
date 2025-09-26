using System;
using System.Collections.Generic;
using MirraGames.SDK.Common;
using UnityEngine;
using UnityEngine.Networking;

namespace MirraGames.SDK.UnityEngine {

    [Provider(typeof(IAssetBundles))]
    public class UnityEngineAssetBundles : CommonAssetBundles {

        private readonly Dictionary<string, AssetBundle> loadedBundles = new();

        protected override void LoadBundleImpl(string bundleTag, string bundleURL, Action<AssetBundle> onSuccess, Action onError = null) {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(bundleURL);
            request.SendWebRequest().completed += operation => {
                if (request.result == UnityWebRequest.Result.Success) {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
                    loadedBundles[bundleTag] = bundle;
                    onSuccess?.Invoke(bundle);
                }
                else {
                    onError?.Invoke();
                }
                request.Dispose();
            };
        }

        protected override void ReleaseBundleImpl(string bundleTag, bool unloadAllObjects) {
            if (loadedBundles.TryGetValue(bundleTag, out AssetBundle bundle)) {
                bundle.Unload(unloadAllObjects);
                loadedBundles.Remove(bundleTag);
            }
        }

    }

}