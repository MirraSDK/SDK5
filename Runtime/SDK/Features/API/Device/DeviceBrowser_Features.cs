using MirraGames.SDK.SourceGenerator;
using MirraGames.SDK.Common;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class DeviceBrowser_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<DeviceBrowser_Features> { }

        public DeviceBrowser_Features() {
            SetInfo("Device Browser", nameof(IDevice), nameof(DeviceBrowserProvider));

            CreateButton(nameof(IDeviceBrowser.OpenUrl), () => {
                MirraSDK.Device.OpenUrl("https://google.com/");
            });
        }

    }

}