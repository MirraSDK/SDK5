using System;

namespace MirraGames.SDK.Common
{
    public class InterstitialParameters
    {
        public Action OnOpen = null;
        public Action<bool> OnClose = null;
        public string PlacementId = string.Empty;
        public int AdsIntervalSeconds = 120;
    }
}