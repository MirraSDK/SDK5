using System;

namespace MirraGames.SDK.Common {

    public interface IInterstitial {

        bool IsInterstitialReady { get; }
        bool IsInterstitialVisible { get; }
        bool IsInterstitialAvailable { get; }

        DateTime? GetLastInterstitialSuccess();
        void InvokeInterstitial(Action onOpen = null, Action<bool> onClose = null);

    }

}