using MirraGames.SDK.Common;
using System;
using UnityEngine;
using Logger = MirraGames.SDK.Common.Logger;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IStreamingAssets))]
    public class FallbackStreamingAssets : CommonStreamingAssets {

        protected override void LoadStreamingAudioClipImpl(string relativePath, AudioType audioType, Action<AudioClip> onSuccess, Action onError = null) {
            Logger.NotImplementedWarning(this, nameof(LoadStreamingAudioClipImpl));
            onError?.Invoke();
        }

        protected override void LoadStreamingTextImpl(string relativePath, Action<string> onSuccess, Action onError = null) {
            Logger.NotImplementedWarning(this, nameof(LoadStreamingTextImpl));
            onError?.Invoke();
        }

        protected override void LoadStreamingTexture2DImpl(string relativePath, Action<Texture2D> onSuccess, Action onError = null) {
            Logger.NotImplementedWarning(this, nameof(LoadStreamingTexture2DImpl));
            onError?.Invoke();
        }

        protected override void LoadStreamingJSONImpl<TSerializable>(string relativePath, Action<TSerializable> onSuccess, Action onError = null) {
            Logger.NotImplementedWarning(this, nameof(LoadStreamingJSONImpl));
            onError?.Invoke();
        }

    }

}