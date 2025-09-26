using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class Flags_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<Flags_Features> { }

        public Flags_Features() {
            SetInfo("Flags", nameof(IFlags), nameof(FlagsProvider));

            CreateButton(nameof(IFlags.GetBool), () => {
                bool customBool = MirraSDK.Flags.GetBool("customBool");
                Debug.Log($"customBool: {customBool}");
            });
            CreateButton(nameof(IFlags.GetInt), () => {
                int customInt = MirraSDK.Flags.GetInt("customInt");
                Debug.Log($"customInt: {customInt}");
            });
            CreateButton(nameof(IFlags.GetFloat), () => {
                float customFloat = MirraSDK.Flags.GetFloat("customFloat");
                Debug.Log($"customFloat: {customFloat}");
            });
            CreateButton(nameof(IFlags.GetString), () => {
                string customString = MirraSDK.Flags.GetString("customString");
                Debug.Log($"customString: {customString}");
            });
            CreateButton(nameof(IFlags.HasKey), () => {
                bool hasKey = MirraSDK.Flags.HasKey("customBool");
                Debug.Log($"hasKey (customBool): {hasKey}");
            });
        }

    }

}