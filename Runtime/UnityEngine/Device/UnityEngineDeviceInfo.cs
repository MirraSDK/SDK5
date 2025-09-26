using MirraGames.SDK.Common;
using UnityEngine;

namespace MirraGames.SDK.UnityEngine {

    [Provider(typeof(IDeviceInfo))]
    public class UnityEngineDeviceInfo : CommonDeviceInfo {

        protected override bool GetIsMobileImpl() {
            return Application.isMobilePlatform;
        }

        protected override SystemType GetSystemTypeImpl() {
            return Application.platform switch {
                RuntimePlatform.Android => SystemType.Android,
                RuntimePlatform.IPhonePlayer => SystemType.iOS,
                RuntimePlatform.WindowsEditor => SystemType.Windows,
                RuntimePlatform.WindowsPlayer => SystemType.Windows,
                RuntimePlatform.LinuxEditor => SystemType.Linux,
                RuntimePlatform.LinuxPlayer => SystemType.Linux,
                RuntimePlatform.OSXEditor => SystemType.Mac,
                RuntimePlatform.OSXPlayer => SystemType.Mac,
                _ => SystemType.Unknown
            };
        }

    }

}