using MirraGames.SDK.Common;
using System;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IPlayerAccount))]
    public class FallbackPlayerAccount : CommonPlayerAccount {

        public FallbackPlayerAccount() {
            SetInitialized();
        }

        protected override string GetDisplayNameImpl() {
            Logger.NotImplementedWarning(this, nameof(GetDisplayNameImpl));
            return default;
        }

        protected override string GetFirstNameImpl() {
            Logger.NotImplementedWarning(this, nameof(GetFirstNameImpl));
            return default;
        }

        protected override string GetLastNameImpl() {
            Logger.NotImplementedWarning(this, nameof(GetLastNameImpl));
            return default;
        }

        protected override string GetUsernameImpl() {
            Logger.NotImplementedWarning(this, nameof(GetUsernameImpl));
            return default;
        }

        protected override string GetUniqueIdImpl() {
            Logger.NotImplementedWarning(this, nameof(GetUniqueIdImpl));
            return default;
        }

        protected override bool IsLoggedInImpl() {
            Logger.NotImplementedWarning(this, nameof(IsLoggedInImpl));
            return default;
        }

        protected override void InvokeLoginImpl(Action onLoginSuccess = null, Action onLoginError = null) {
            Logger.NotImplementedWarning(this, nameof(InvokeLoginImpl));
            onLoginError?.Invoke();
        }

    }

}