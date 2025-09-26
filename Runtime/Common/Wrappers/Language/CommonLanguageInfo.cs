using System;

namespace MirraGames.SDK.Common {

    [Awaitable, Wrapper]
    public abstract partial class CommonLanguageInfo : ILanguageInfo {

        protected abstract LanguageType GetCurrentImpl();

        public LanguageType Current {
            get {
                try {
                    return GetCurrentImpl();
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(Current), exception);
                    return LanguageType.English;
                }
            }
        }

    }

}