using System;

namespace MirraGames.SDK.Common {

    [Module]
    public partial interface IDateTime {

        DateTime CurrentDate { get; }
        HolidayType CurrentHoliday { get; }

    }

}