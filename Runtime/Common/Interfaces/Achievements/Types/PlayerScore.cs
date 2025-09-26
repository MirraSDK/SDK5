using System;
using UnityEngine;

namespace MirraGames.SDK.Common {

    [Serializable]
    public record PlayerScore {

        [SerializeField] public string displayName;
        [SerializeField] public int position;
        [SerializeField] public int score;
        [SerializeField] public string profilePictureUrl;

    }

}