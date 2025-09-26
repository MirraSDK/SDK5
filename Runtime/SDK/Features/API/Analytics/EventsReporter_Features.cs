using System.Collections.Generic;
using MirraGames.SDK.SourceGenerator;
using MirraGames.SDK.Common;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class EventsReporter_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<EventsReporter_Features> { }

        public EventsReporter_Features() {
            SetInfo("Events Reporter", nameof(IAnalytics), nameof(EventsReporterProvider));

            CreateButton($"{nameof(IEventsReporter.Report)} event", () => {
                MirraSDK.Analytics.Report("customEvent");
            });
            CreateButton($"{nameof(IEventsReporter.Report)} event value", () => {
                MirraSDK.Analytics.Report("customEvent", "customValue");
            });
            CreateButton($"{nameof(IEventsReporter.Report)} event parameters", () => {
                MirraSDK.Analytics.Report("customEvent", new Dictionary<string, object> {
                    { "customKey1", "customValue1" },
                    { "customKey2", "customValue2" },
                    { "customKey3", "customValue3" },
                    { "customKey4", "customValue4" }
                });
            });
        }

    }

}