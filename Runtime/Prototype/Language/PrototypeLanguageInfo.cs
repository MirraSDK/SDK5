using MirraGames.SDK.Common;

namespace MirraGames.SDK.Prototype {

    [Provider(typeof(ILanguageInfo))]
    public class PrototypeLanguageInfo : CommonLanguageInfo {

        private readonly PrototypeLanguageInfo_Configuration config;

        public PrototypeLanguageInfo(PrototypeLanguageInfo_Configuration config) {
            this.config = config;
            SetInitialized();
        }

        protected override LanguageType GetCurrentImpl() {
            return config.Current;
        }

    }

}