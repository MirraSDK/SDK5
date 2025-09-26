using MirraGames.SDK.Common;

namespace MirraGames.SDK.Prototype {

    [Provider(typeof(IPlatformInfo))]
    public class PrototypePlatformInfo : CommonPlatformInfo {

        private readonly PrototypePlatform_Configuration config;

        public PrototypePlatformInfo(PrototypePlatform_Configuration config) {
            this.config = config;
        }

        protected override string GetAppIdImpl() {
            return config.AppId;
        }

        protected override PlatformType GetCurrentImpl() {
            return config.PlatformType;
        }

        protected override DeploymentType GetDeploymentImpl() {
            return config.DeploymentType;
        }

    }

}