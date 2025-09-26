using MirraGames.SDK.Common;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(ILanguageInfo))]
    public class FallbackLanguageInfo : CommonLanguageInfo {

        public FallbackLanguageInfo() {
            SetInitialized();
        }

        protected override LanguageType GetCurrentImpl() {
            Logger.NotImplementedWarning(this, nameof(GetCurrentImpl));
            return default;
        }

    }

}