using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class DeviceCursor_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<DeviceCursor_Features> { }

        public DeviceCursor_Features() {
            SetInfo("Device Cursor", nameof(IDevice), nameof(DeviceCursorProvider));

            CreateBoolean(nameof(IDeviceCursor.CursorVisible), () => {
                return MirraSDK.Device.CursorVisible;
            });
            CreateString(nameof(IDeviceCursor.CursorLock), () => {
                return MirraSDK.Device.CursorLock.ToString();
            });
            CreateButton("Show Cursor", () => {
                MirraSDK.Device.CursorVisible = true;
            });
            CreateButton("Hide Cursor", () => {
                MirraSDK.Device.CursorVisible = false;
            });
        }

    }

}