using System;

namespace MirraGames.SDK.Common {

    [Awaitable, Wrapper]
    public abstract partial class CommonPlayerAccount : IPlayerAccount {

        protected abstract string GetDisplayNameImpl();
        protected abstract string GetFirstNameImpl();
        protected abstract string GetLastNameImpl();
        protected abstract string GetUsernameImpl();
        protected abstract string GetUniqueIdImpl();
        protected abstract bool IsLoggedInImpl();
        protected abstract void InvokeLoginImpl(Action onLoginSuccess = null, Action onLoginError = null);

        public string DisplayName {
            get {
                try {
                    return GetDisplayNameImpl();
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(DisplayName), exception);
                    return string.Empty;
                }
            }
        }

        public string FirstName {
            get {
                try {
                    return GetFirstNameImpl();
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(FirstName), exception);
                    return string.Empty;
                }
            }
        }

        public string LastName {
            get {
                try {
                    return GetLastNameImpl();
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(LastName), exception);
                    return string.Empty;
                }
            }
        }

        public string Username {
            get {
                try {
                    return GetUsernameImpl();
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(Username), exception);
                    return string.Empty;
                }
            }
        }

        public string UniqueId {
            get {
                try {
                    return GetUniqueIdImpl();
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(UniqueId), exception);
                    return string.Empty;
                }
            }
        }

        public bool IsLoggedIn {
            get {
                try {
                    return IsLoggedInImpl();
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(IsLoggedIn), exception);
                    return false;
                }
            }
        }

        public void InvokeLogin(Action onLoginSuccess = null, Action onLoginError = null) {
            Logger.CreateText(this, nameof(InvokeLogin));
            try {
                void onLoginSuccessCallback() {
                    Logger.CreateText(this, nameof(onLoginSuccessCallback));
                    onLoginSuccess?.Invoke();
                }
                void onLoginErrorCallback() {
                    Logger.CreateText(this, nameof(onLoginErrorCallback));
                    onLoginError?.Invoke();
                }
                InvokeLoginImpl(onLoginSuccessCallback, onLoginErrorCallback);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(InvokeLogin), exception);
                onLoginError?.Invoke();
            }
        }

    }

}