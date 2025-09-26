using MirraGames.SDK.Common;
using UnityEngine;

namespace MirraGames.SDK.UnityEngine {

    [Provider(typeof(IDeviceBrowser))]
    public class UnityEngineDeviceBrowser : CommonDeviceBrowser {

        protected override void OpenUrlImpl(string url) {
            Application.OpenURL(url);
        }

    }

}