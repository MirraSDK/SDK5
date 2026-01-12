using System;

namespace MirraGames.SDK.Common
{
    public class RewardedParameters
    {
        public Action OnOpen = null;
        public Action<bool> OnClose = null;
        public string PlacementId = string.Empty;
    }
}