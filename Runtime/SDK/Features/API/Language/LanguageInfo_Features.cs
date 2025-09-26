using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class LanguageInfo_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<LanguageInfo_Features> { }

        public LanguageInfo_Features() {
            SetInfo("Language Info", nameof(ILanguage), nameof(LanguageInfoProvider));

            CreateString(nameof(ILanguage.Current), () => {
                return MirraSDK.Language.Current.ToString();
            });
        }

    }

}