using System;
using UnityEngine;

namespace MirraGames.SDK.Common {

    [Wrapper]
    public abstract partial class CommonAssetBundles : IAssetBundles {

        protected abstract void LoadBundleImpl(string bundleTag, string bundleURL, Action<AssetBundle> onSuccess, Action onError = null);
        protected abstract void ReleaseBundleImpl(string bundleTag, bool unloadAllObjects);

        public void LoadBundle(string bundleTag, string bundleURL, Action<AssetBundle> onSuccess, Action onError = null) {
            Logger.CreateText(this, nameof(LoadBundle), bundleTag, bundleURL);
            try {
                LoadBundleImpl(bundleTag, bundleURL, onSuccess, onError);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(LoadBundle), exception);
            }
        }

        public void ReleaseBundle(string bundleTag, bool unloadAllObjects) {
            Logger.CreateText(this, nameof(ReleaseBundle), bundleTag, unloadAllObjects);
            try {
                ReleaseBundleImpl(bundleTag, unloadAllObjects);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(ReleaseBundle), exception);
            }
        }

    }

}