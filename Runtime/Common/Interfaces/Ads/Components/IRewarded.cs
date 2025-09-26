using System;

namespace MirraGames.SDK.Common {

    public interface IRewarded {

        bool IsRewardedReady { get; }
        bool IsRewardedVisible { get; }
        bool IsRewardedAvailable { get; }

        DateTime? GetLastRewardedSuccess(string rewardTag = null);
        void InvokeRewarded(Action onOpen = null, Action<bool> onClose = null, string rewardTag = null);

    }

}