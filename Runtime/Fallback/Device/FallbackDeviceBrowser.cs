using MirraGames.SDK.Common;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IDeviceBrowser))]
    public class FallbackDeviceBrowser : CommonDeviceBrowser {

        protected override void OpenUrlImpl(string url) {
            Logger.NotImplementedWarning(this, nameof(OpenUrlImpl));
        }

    }

}