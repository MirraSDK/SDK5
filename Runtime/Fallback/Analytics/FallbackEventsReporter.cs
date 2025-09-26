using MirraGames.SDK.Common;
using System.Collections.Generic;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IEventsReporter))]
    public class FallbackEventsReporter : CommonEventsReporter {

        public FallbackEventsReporter() {
            SetInitialized();
        }

        protected override void ReportImpl(string eventName) {
            Logger.NotImplementedWarning(this, nameof(ReportImpl));
        }

        protected override void ReportImpl(string eventName, string eventValue) {
            Logger.NotImplementedWarning(this, nameof(ReportImpl));
        }

        protected override void ReportImpl(string eventName, Dictionary<string, object> eventParameters) {
            Logger.NotImplementedWarning(this, nameof(ReportImpl));
        }

    }

}