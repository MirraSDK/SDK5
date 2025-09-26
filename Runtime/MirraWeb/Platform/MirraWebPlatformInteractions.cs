using MirraGames.SDK.Common;
using System.Runtime.InteropServices;

namespace MirraGames.SDK.MirraWeb {

    [Provider(typeof(IPlatformInteractions))]
    public class MirraWebPlatformInteractions : CommonPlatformInteractions {

        [DllImport(Naming.InternalDll)] private static extern void mirraSDK_platform_shareGame(string messageText);
        [DllImport(Naming.InternalDll)] private static extern void mirraSDK_platform_rateGame();

        protected override void ShareGameImpl(string messageText) {
            mirraSDK_platform_shareGame(messageText);
        }

        protected override void RateGameImpl() {
            mirraSDK_platform_rateGame();
        }

    }

}