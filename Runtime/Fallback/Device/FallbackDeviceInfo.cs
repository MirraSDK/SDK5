using MirraGames.SDK.Common;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IDeviceInfo))]
    public class FallbackDeviceInfo : CommonDeviceInfo {

        protected override bool GetIsMobileImpl() {
            Logger.NotImplementedWarning(this, nameof(GetIsMobileImpl));
            return default;
        }

        protected override SystemType GetSystemTypeImpl() {
            Logger.NotImplementedWarning(this, nameof(GetSystemTypeImpl));
            return default;
        }

    }

}