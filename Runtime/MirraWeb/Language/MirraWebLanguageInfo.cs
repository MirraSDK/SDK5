using MirraGames.SDK.Common;
using System.Runtime.InteropServices;

namespace MirraGames.SDK.MirraWeb {

    [Provider(typeof(ILanguageInfo))]
    public class MirraWebLanguageInfo : CommonLanguageInfo {

        [DllImport(Naming.InternalDll)] private static extern int mirraSDK_language_current();

        public MirraWebLanguageInfo() {
            SetInitialized();
        }

        protected override LanguageType GetCurrentImpl() {
            return (LanguageType)mirraSDK_language_current();
        }

    }

}