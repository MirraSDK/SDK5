using MirraGames.SDK.Common;
using System.Runtime.InteropServices;

namespace MirraGames.SDK.MirraWeb {

    [Provider(typeof(IPlatformInfo))]
    public class MirraWebPlatformInfo : CommonPlatformInfo {

        [DllImport(Naming.InternalDll)] private static extern string mirraSDK_platform_current();
        [DllImport(Naming.InternalDll)] private static extern string mirraSDK_platform_appId();

        protected override PlatformType GetCurrentImpl() {
            string platformName = mirraSDK_platform_current();
            return platformName.ToEnumOrDefault<PlatformType>(PlatformType.Unknown);
        }

        protected override DeploymentType GetDeploymentImpl() {
            return DeploymentType.Web;
        }

        protected override string GetAppIdImpl() {
            return mirraSDK_platform_appId();
        }

    }

}