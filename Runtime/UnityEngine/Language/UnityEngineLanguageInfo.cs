using MirraGames.SDK.Common;
using UnityEngine;

namespace MirraGames.SDK.UnityEngine {

    [Provider(typeof(ILanguageInfo))]
    public class UnityEngineLanguageInfo : CommonLanguageInfo {

        protected override LanguageType GetCurrentImpl() {
            return Application.systemLanguage switch {
                SystemLanguage.Russian => LanguageType.Russian,
                SystemLanguage.Japanese => LanguageType.Japanese,
                SystemLanguage.Chinese => LanguageType.Chinese,
                SystemLanguage.ChineseSimplified => LanguageType.Chinese,
                SystemLanguage.ChineseTraditional => LanguageType.Chinese,
                SystemLanguage.Turkish => LanguageType.Turkish,
#if UNITY_2022_3_OR_NEWER
                SystemLanguage.Hindi => LanguageType.Hindi,
#endif
                SystemLanguage.Korean => LanguageType.Korean,
                SystemLanguage.Portuguese => LanguageType.Portuguese,
                SystemLanguage.Indonesian => LanguageType.Indonesian,
                SystemLanguage.German => LanguageType.German,
                SystemLanguage.Spanish => LanguageType.Spanish,
                SystemLanguage.Italian => LanguageType.Italian,
                SystemLanguage.Ukrainian => LanguageType.Ukrainian,
                SystemLanguage.Polish => LanguageType.Polish,
                SystemLanguage.French => LanguageType.French,
                _ => LanguageType.English
            };
        }

    }

}