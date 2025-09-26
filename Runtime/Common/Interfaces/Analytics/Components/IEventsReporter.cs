using System.Collections.Generic;

namespace MirraGames.SDK.Common {

    [Awaitable, Module]
    public partial interface IEventsReporter {

        void Report(string eventName);
        void Report(string eventName, string eventValue);
        void Report(string eventName, Dictionary<string, object> eventParameters);

    }

}