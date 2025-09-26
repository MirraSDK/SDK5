using System;

namespace MirraGames.SDK.Common {

    [Wrapper]
    public abstract partial class CommonDeviceBrowser : IDeviceBrowser {

        protected abstract void OpenUrlImpl(string url);

        public void OpenUrl(string url) {
            Logger.CreateText(this, nameof(OpenUrl), url);
            try {
                OpenUrlImpl(url);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(OpenUrl), exception);
            }
        }

    }

}