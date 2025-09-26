using System;
using UnityEngine;

namespace MirraGames.SDK.Common {

    [Module]
    public partial interface IAssetBundles {

        void LoadBundle(string bundleTag, string bundleURL, Action<AssetBundle> onSuccess, Action onError = null);
        void ReleaseBundle(string bundleTag, bool unloadAllObjects);

    }

}