using System;

namespace MirraGames.SDK.Common {

    [Awaitable, Module]
    public partial interface IPlayerAccount {

        string DisplayName { get; }
        string FirstName { get; }
        string LastName { get; }
        string Username { get; }
        string UniqueId { get; }
        bool IsLoggedIn { get; }

        void InvokeLogin(Action onLoginSuccess = null, Action onLoginError = null);

    }

}