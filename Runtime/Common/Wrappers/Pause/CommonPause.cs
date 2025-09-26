using System.Collections.Generic;

namespace MirraGames.SDK.Common {

    [Wrapper]
    public abstract partial class CommonPause : IPause, IEventListener<PauseSourceEvent> {

        protected readonly IEventAggregator eventAggregator;
        protected readonly HashSet<string> pauseSources = new();

        public CommonPause(IEventAggregator eventAggregator) {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe<PauseSourceEvent>(this);
        }

        public void OnEvent(PauseSourceEvent eventData) {
            Register(eventData.Source, eventData.Pause);
        }

        public virtual void OnPauseChange(bool isPaused) { }

        public void Register(object source, bool value) {
            string key = (source is string) ? source as string : source.GetType().ToString();
            if (value) {
                pauseSources.Add(key);
                Logger.CreateText(this, $"Pause from {key}", $"[{string.Join(", ", pauseSources)}]");
            }
            else {
                pauseSources.Remove(key);
                Logger.CreateText(this, $"Unpause from {key}", $"[{string.Join(", ", pauseSources)}]");
            }
            bool isPaused = pauseSources.Count > 0;
            eventAggregator.Publish(this, new PauseChangeEvent(isPaused));
            OnPauseChange(isPaused);
        }

    }

}