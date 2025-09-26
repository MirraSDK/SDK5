using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class Data_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<Data_Features> { }

        public Data_Features() {
            SetInfo("Data", nameof(IData), nameof(DataProvider));

            CreateButton(nameof(IDataContainer.GetBool), () => {
                bool value = MirraSDK.Data.GetBool("customBool");
                Debug.Log($"get customBool {value}");
            });
            CreateButton(nameof(IDataContainer.SetBool), () => {
                bool value = Random.Range(0, 2) == 0;
                MirraSDK.Data.SetBool("customBool", value);
                Debug.Log($"set customBool {value}");
            });

            CreateButton(nameof(IDataContainer.GetInt), () => {
                int value = MirraSDK.Data.GetInt("customInt");
                Debug.Log($"get customInt {value}");
            });
            CreateButton(nameof(IDataContainer.SetInt), () => {
                int value = Random.Range(0, int.MaxValue);
                MirraSDK.Data.SetInt("customInt", value);
                Debug.Log($"set customInt {value}");
            });

            CreateButton(nameof(IDataContainer.GetFloat), () => {
                float value = MirraSDK.Data.GetFloat("customFloat");
                Debug.Log($"get customFloat '{value}'");
            });
            CreateButton(nameof(IDataContainer.SetFloat), () => {
                float value = Random.Range(0f, float.MaxValue);
                MirraSDK.Data.SetFloat("customFloat", value);
                Debug.Log($"set customFloat '{value}'");
            });

            CreateButton(nameof(IDataContainer.GetString), () => {
                string value = MirraSDK.Data.GetString("customString");
                Debug.Log($"get customString '{value}'");
            });
            CreateButton(nameof(IDataContainer.SetString), () => {
                // Encode random integer into base64 string
                int randomInt = Random.Range(0, int.MaxValue);
                string value = global::System.Convert.ToBase64String(global::System.BitConverter.GetBytes(randomInt));
                MirraSDK.Data.SetString("customString", value);
                Debug.Log($"set customString '{value}'");
            });

            CreateButton(nameof(IDataContainer.GetObject), () => {
                Vector3 value = MirraSDK.Data.GetObject<Vector3>("customSerializable");
                Debug.Log($"get customSerializable {value}");
            });
            CreateButton(nameof(IDataContainer.SetObject), () => {
                Vector3 value = new(
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f));
                MirraSDK.Data.SetObject("customSerializable", value);
                Debug.Log($"set customSerializable {value}");
            });

            CreateButton(nameof(IDataContainer.Save), () => {
                MirraSDK.Data.Save();
            });
            CreateButton(nameof(IDataContainer.HasKey), () => {
                bool value = MirraSDK.Data.HasKey("customBool");
                Debug.Log($"HasKey customBool {value}");
            });
            CreateButton(nameof(IDataContainer.DeleteKey), () => {
                MirraSDK.Data.DeleteKey("customKey");
            });
            CreateButton(nameof(IDataContainer.DeleteAll), () => {
                MirraSDK.Data.DeleteAll();
            });
        }

    }

}