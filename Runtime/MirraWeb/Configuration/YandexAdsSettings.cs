using MirraGames.SDK.Common;
using System;
using UnityEngine;

namespace MirraGames.SDK.MirraWeb {

    [Serializable]
    public class YandexAdsSettings {
        [SerializeField] public string appId = "";
        [SerializeField] public string blockIdsJson = Naming.EmptyJson;
        [SerializeField] public float interstitialInterval = 0;
    }

}