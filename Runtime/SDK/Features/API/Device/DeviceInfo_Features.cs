using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class DeviceInfo_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<DeviceInfo_Features> { }

        public DeviceInfo_Features() {
            SetInfo("Device Info", nameof(IDevice), nameof(DeviceInfoProvider));

            CreateBoolean(nameof(IDeviceInfo.IsMobile), () => {
                return MirraSDK.Device.IsMobile;
            });
            CreateString(nameof(IDeviceInfo.SystemType), () => {
                return MirraSDK.Device.SystemType.ToString();
            });
        }

    }

}