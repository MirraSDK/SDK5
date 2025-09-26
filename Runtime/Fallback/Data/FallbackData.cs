using MirraGames.SDK.Common;
using System;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IData))]
    public class FallbackData : CommonData {

        public FallbackData(IEventDispatcher eventDispatcher) : base(eventDispatcher) {
            ReadJson(ParseContainers);
        }

        protected override void ReadJson(Action<string> jsonRequest) {
            jsonRequest?.Invoke(Naming.EmptyJson);
        }

        protected override void WriteJson(string json) {
            Logger.NotImplementedWarning(this, nameof(WriteJson));
        }

    }

}