using System;
using UnityEngine;

namespace MirraGames.SDK.Common {

    [Wrapper]
    public abstract partial class CommonStreamingAssets : IStreamingAssets {

        protected abstract void LoadStreamingAudioClipImpl(string relativePath, AudioType audioType, Action<AudioClip> onSuccess, Action onError = null);
        protected abstract void LoadStreamingTextImpl(string relativePath, Action<string> onSuccess, Action onError = null);
        protected abstract void LoadStreamingTexture2DImpl(string relativePath, Action<Texture2D> onSuccess, Action onError = null);
        protected abstract void LoadStreamingJSONImpl<TSerializable>(string relativePath, Action<TSerializable> onSuccess, Action onError = null);

        public void LoadStreamingAudioClip(string relativePath, AudioType audioType, Action<AudioClip> onSuccess, Action onError = null) {
            Logger.CreateText(this, nameof(LoadStreamingAudioClip), relativePath, audioType);
            try {
                LoadStreamingAudioClipImpl(relativePath, audioType, onSuccess, onError);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(LoadStreamingAudioClip), exception);
            }
        }

        public void LoadStreamingText(string relativePath, Action<string> onSuccess, Action onError = null) {
            Logger.CreateText(this, nameof(LoadStreamingText), relativePath);
            try {
                LoadStreamingTextImpl(relativePath, onSuccess, onError);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(LoadStreamingText), exception);
            }
        }

        public void LoadStreamingTexture2D(string relativePath, Action<Texture2D> onSuccess, Action onError = null) {
            Logger.CreateText(this, nameof(LoadStreamingTexture2D), relativePath);
            try {
                LoadStreamingTexture2DImpl(relativePath, onSuccess, onError);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(LoadStreamingTexture2D), exception);
            }
        }

        public void LoadStreamingJSON<TSerializable>(string relativePath, Action<TSerializable> onSuccess, Action onError = null) {
            Logger.CreateText(this, nameof(LoadStreamingJSON), relativePath, typeof(TSerializable).Name);
            try {
                LoadStreamingJSONImpl(relativePath, onSuccess, onError);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(LoadStreamingJSON), exception);
            }
        }

    }

}