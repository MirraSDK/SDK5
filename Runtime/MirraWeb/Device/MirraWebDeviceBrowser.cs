using MirraGames.SDK.Common;
using System.Runtime.InteropServices;

namespace MirraGames.SDK.MirraWeb {

    [Provider(typeof(IDeviceBrowser))]
    public class MirraWebDeviceBrowser : CommonDeviceBrowser {

        [DllImport(Naming.InternalDll)] private static extern void mirraSDK_device_openUrl(string url);

        protected override void OpenUrlImpl(string url) {
            mirraSDK_device_openUrl(url);
        }

    }

}