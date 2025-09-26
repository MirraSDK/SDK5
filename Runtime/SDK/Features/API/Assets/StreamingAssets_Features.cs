using MirraGames.SDK.SourceGenerator;
using MirraGames.SDK.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class StreamingAssets_Features : FeaturesContainer {

        private static bool isAudioClipClicked = false;

        public new class UxmlFactory : UxmlFactory<StreamingAssets_Features> { }

        public StreamingAssets_Features() {
            SetInfo("Streaming Assets", nameof(IAssets), nameof(StreamingAssetsProvider));

            CreateButton(nameof(IStreamingAssets.LoadStreamingAudioClip), () => {
                if (isAudioClipClicked) {
                    Debug.Log("load audio is already clicked");
                    return;
                }
                isAudioClipClicked = true;
                MirraSDK.Assets.LoadStreamingAudioClip(Application.streamingAssetsPath + "/lostdata1990.mp3", AudioType.MPEG, onSuccess: (audioClip) => {
                    Debug.Log("load audio success");
                    GameObject go = new();
                    go.AddComponent<AudioSource>().PlayOneShot(audioClip);
                },
                onError: () => {
                    Debug.LogError("load audio error");
                });
            });
            CreateButton(nameof(IStreamingAssets.LoadStreamingText), () => {
                MirraSDK.Assets.LoadStreamingText("customText", onSuccess: (text) => {
                    Debug.Log($"Resolved text: {text}");
                },
                onError: () => {
                    Debug.LogError("Failed to resolve text.");
                });
            });
            CreateButton(nameof(IStreamingAssets.LoadStreamingTexture2D), () => {
                MirraSDK.Assets.LoadStreamingTexture2D("customTexture2D", onSuccess: (texture2D) => {
                    Debug.Log($"Resolved texture: {texture2D.name}");
                },
                onError: () => {
                    Debug.LogError("Failed to resolve texture.");
                });
            });
            CreateButton(nameof(IStreamingAssets.LoadStreamingJSON), () => {
                MirraSDK.Assets.LoadStreamingJSON<Vector3>("customJSON", onSuccess: (customSerializable) => {
                    Debug.Log($"Resolved JSON: {customSerializable}");
                },
                onError: () => {
                    Debug.LogError("Failed to resolve JSON.");
                });
            });
        }

    }

}