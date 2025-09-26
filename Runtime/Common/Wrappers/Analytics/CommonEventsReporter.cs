using System;
using System.Collections.Generic;

namespace MirraGames.SDK.Common {

    [Awaitable, Wrapper]
    public abstract partial class CommonEventsReporter : IEventsReporter {

        protected abstract void ReportImpl(string eventName);
        protected abstract void ReportImpl(string eventName, string eventValue);
        protected abstract void ReportImpl(string eventName, Dictionary<string, object> eventParameters);

        public void Report(string eventName) {
            Logger.CreateText(this, nameof(Report), eventName);
            try {
                ReportImpl(eventName);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(Report), exception);
            }
        }

        public void Report(string eventName, string eventValue) {
            Logger.CreateText(this, nameof(Report), eventName, eventValue);
            try {
                ReportImpl(eventName, eventValue);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(Report), exception);
            }
        }

        public void Report(string eventName, Dictionary<string, object> eventParameters) {
            Logger.CreateText(this, nameof(Report), eventName, eventParameters);
            try {
                ReportImpl(eventName, eventParameters);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(Report), exception);
            }
        }

    }

}