using System;
using MirraGames.SDK.Common;
using UnityEngine;
using UnityEngine.Networking;

namespace MirraGames.SDK.UnityEngine {

    [Provider(typeof(IStreamingAssets))]
    public class UnityEngineStreamingAssets : CommonStreamingAssets {

        protected override void LoadStreamingAudioClipImpl(string relativePath, AudioType audioType, Action<AudioClip> onSuccess, Action onError = null) {
            string url = relativePath;
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, audioType);
            request.SendWebRequest().completed += operation => {
                if (request.result == UnityWebRequest.Result.Success) {
                    AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
                    onSuccess?.Invoke(audioClip);
                }
                else {
                    onError?.Invoke();
                }
                request.Dispose();
            };
        }

        protected override void LoadStreamingTextImpl(string relativePath, Action<string> onSuccess, Action onError = null) {
            string url = relativePath;
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SendWebRequest().completed += operation => {
                if (request.result == UnityWebRequest.Result.Success) {
                    string text = request.downloadHandler.text;
                    onSuccess?.Invoke(text);
                }
                else {
                    onError?.Invoke();
                }
                request.Dispose();
            };
        }

        protected override void LoadStreamingTexture2DImpl(string relativePath, Action<Texture2D> onSuccess, Action onError = null) {
            string url = relativePath;
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            request.SendWebRequest().completed += operation => {
                if (request.result == UnityWebRequest.Result.Success) {
                    Texture2D texture = DownloadHandlerTexture.GetContent(request);
                    onSuccess?.Invoke(texture);
                }
                else {
                    onError?.Invoke();
                }
                request.Dispose();
            };
        }

        protected override void LoadStreamingJSONImpl<TSerializable>(string relativePath, Action<TSerializable> onSuccess, Action onError = null) {
            LoadStreamingText(relativePath, text => {
                try {
                    TSerializable obj = JsonUtility.FromJson<TSerializable>(text);
                    onSuccess?.Invoke(obj);
                }
                catch {
                    onError?.Invoke();
                }
            }, onError);
        }

    }

}