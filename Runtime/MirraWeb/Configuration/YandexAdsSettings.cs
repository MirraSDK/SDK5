using System;
using UnityEngine;

namespace MirraGames.SDK.MirraWeb {

    [Serializable]
    public class YandexAdsSettings {
        [SerializeField] public string appId = "";
        [SerializeField] public string interstitialMobileId = "";
        [SerializeField] public string interstitialDesktopId = "";
        [SerializeField] public string rewardedMobileId = "";
        [SerializeField] public string rewardedDesktopId = "";
        [SerializeField] public float interstitialInterval = 0;
    }

}