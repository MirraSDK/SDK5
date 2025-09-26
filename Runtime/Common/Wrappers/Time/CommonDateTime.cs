using System;

namespace MirraGames.SDK.Common {

    public abstract partial class CommonDateTime : IDateTime {

        protected abstract DateTime GetCurrentDateImpl();
        protected abstract HolidayType GetCurrentHolidayImpl();

        public DateTime CurrentDate {
            get {
                try {
                    return GetCurrentDateImpl();
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(CurrentDate), exception);
                    return DateTime.MinValue;
                }
            }
        }

        public HolidayType CurrentHoliday {
            get {
                try {
                    return GetCurrentHolidayImpl();
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(CurrentHoliday), exception);
                    return HolidayType.None;
                }
            }
        }

    }

}