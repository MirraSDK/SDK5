using MirraGames.SDK.Common;
using System;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IDateTime))]
    public class FallbackDateTime : CommonDateTime {

        protected override DateTime GetCurrentDateImpl() {
            Logger.NotImplementedWarning(this, nameof(GetCurrentDateImpl));
            return default;
        }

        protected override HolidayType GetCurrentHolidayImpl() {
            Logger.NotImplementedWarning(this, nameof(GetCurrentHolidayImpl));
            return default;
        }

    }

}