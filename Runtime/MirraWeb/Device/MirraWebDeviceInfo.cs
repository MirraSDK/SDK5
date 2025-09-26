using MirraGames.SDK.Common;
using System.Runtime.InteropServices;

namespace MirraGames.SDK.MirraWeb {

    [Provider(typeof(IDeviceInfo))]
    public class MirraWebDeviceInfo : CommonDeviceInfo {

        [DllImport(Naming.InternalDll)] private static extern bool mirraSDK_device_isMobile();
        [DllImport(Naming.InternalDll)] private static extern string mirraSDK_device_systemType();

        protected override bool GetIsMobileImpl() {
            return mirraSDK_device_isMobile();
        }

        protected override SystemType GetSystemTypeImpl() {
            string systemType = mirraSDK_device_systemType();
            return systemType.ToEnumOrDefault<SystemType>(SystemType.Unknown);
        }

    }

}