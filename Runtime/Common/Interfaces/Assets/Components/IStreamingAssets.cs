using System;
using UnityEngine;

namespace MirraGames.SDK.Common {

    [Module]
    public partial interface IStreamingAssets {

        void LoadStreamingAudioClip(string relativePath, AudioType audioType, Action<AudioClip> onSuccess, Action onError = null);
        void LoadStreamingText(string relativePath, Action<string> onSuccess, Action onError = null);
        void LoadStreamingTexture2D(string relativePath, Action<Texture2D> onSuccess, Action onError = null);
        void LoadStreamingJSON<TSerializable>(string relativePath, Action<TSerializable> onSuccess, Action onError = null);

    }

}