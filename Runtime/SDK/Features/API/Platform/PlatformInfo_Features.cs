using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class PlatformInfo_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<PlatformInfo_Features> { }

        public PlatformInfo_Features() {
            SetInfo("Platform Info", nameof(IPlatform), nameof(PlatformInfoProvider));

            CreateString(nameof(IPlatform.Current), () => {
                return MirraSDK.Platform.Current.ToString();
            });

            CreateString(nameof(IPlatform.Deployment), () => {
                return MirraSDK.Platform.Deployment.ToString();
            });

            CreateString(nameof(IPlatform.AppId), () => {
                return MirraSDK.Platform.AppId;
            });

            CreateButton(nameof(IPlatform.ShareGame), () => {
                MirraSDK.Platform.ShareGame("this is example of message text");
            });

            CreateButton(nameof(IPlatform.RateGame), () => {
                MirraSDK.Platform.RateGame();
            });
        }

    }

}