using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class DateTime_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<DateTime_Features> { }

        public DateTime_Features() {
            SetInfo("Date Time", nameof(ITime), nameof(DateTimeProvider));

            CreateString(nameof(IDateTime.CurrentDate), () => {
                return MirraSDK.Time.CurrentDate.ToString("dd/MM/yyyy\nHH:mm:ss");
            });
            CreateString(nameof(IDateTime.CurrentHoliday), () => {
                return MirraSDK.Time.CurrentHoliday.ToString();
            });
        }

    }

}