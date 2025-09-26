using System;
using UnityEngine;

namespace MirraGames.SDK.MirraWeb {

    [Serializable]
    public class AdSenseSettings {
        [SerializeField] public string dataAdClient = "";
        [SerializeField] public string dataAdChannel = "";
        [SerializeField] public bool dataAdBreakTest = false;
        [SerializeField] public float interstitialInterval = 0;
    }

}