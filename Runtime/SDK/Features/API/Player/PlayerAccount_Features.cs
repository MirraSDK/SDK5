using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class PlayerAccount_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<PlayerAccount_Features> { }

        public PlayerAccount_Features() {
            SetInfo("Player Account", nameof(IPlayer), nameof(PlayerAccountProvider));

            CreateString(nameof(IPlayerAccount.DisplayName), () => {
                return MirraSDK.Player.DisplayName;
            });
            CreateString(nameof(IPlayerAccount.FirstName), () => {
                return MirraSDK.Player.FirstName;
            });
            CreateString(nameof(IPlayerAccount.LastName), () => {
                return MirraSDK.Player.LastName;
            });
            CreateString(nameof(IPlayerAccount.Username), () => {
                return MirraSDK.Player.Username;
            });
            CreateString(nameof(IPlayerAccount.UniqueId), () => {
                return MirraSDK.Player.UniqueId;
            });
            CreateBoolean(nameof(IPlayerAccount.IsLoggedIn), () => {
                return MirraSDK.Player.IsLoggedIn;
            });
            CreateButton(nameof(IPlayerAccount.InvokeLogin), () => {
                MirraSDK.Player.InvokeLogin(onLoginSuccess: () => {
                    Debug.Log("Login successful.");
                },
                onLoginError: () => {
                    Debug.LogError("Login failed.");
                });
            });
        }

    }

}