using System;

namespace MirraGames.SDK.Common {

    [Wrapper]
    public abstract partial class CommonPlatformInfo : IPlatformInfo {

        protected abstract PlatformType GetCurrentImpl();
        protected abstract DeploymentType GetDeploymentImpl();
        protected abstract string GetAppIdImpl();

        public PlatformType Current {
            get {
                try {
                    return GetCurrentImpl();
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(Current), exception);
                    return PlatformType.Unknown;
                }
            }
        }

        public DeploymentType Deployment {
            get {
                try {
                    return GetDeploymentImpl();
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(Deployment), exception);
                    return DeploymentType.Unknown;
                }
            }
        }

        public string AppId {
            get {
                try {
                    return GetAppIdImpl();
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(AppId), exception);
                    return string.Empty;
                }
            }
        }

    }

}