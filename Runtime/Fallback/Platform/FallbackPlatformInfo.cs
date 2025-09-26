using MirraGames.SDK.Common;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IPlatformInfo))]
    public class FallbackPlatformInfo : CommonPlatformInfo {

        protected override PlatformType GetCurrentImpl() {
            Logger.NotImplementedWarning(this, nameof(GetCurrentImpl));
            return default;
        }

        protected override DeploymentType GetDeploymentImpl() {
            Logger.NotImplementedWarning(this, nameof(GetDeploymentImpl));
            return default;
        }

        protected override string GetAppIdImpl() {
            Logger.NotImplementedWarning(this, nameof(GetAppIdImpl));
            return default;
        }

    }

}