namespace MirraGames.SDK.Common {

    public abstract partial class CommonTimeScale : ITimeScale, IEventListener<PauseChangeEvent> {

        protected readonly IEventAggregator eventAggregator;

        private bool isPaused = false;
        private float bufferScale;

        public CommonTimeScale(IEventAggregator eventAggregator) {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe<PauseChangeEvent>(this);
            try {
                bufferScale = GetScaleImpl();
            }
            catch (System.Exception exception) {
                Logger.CreateError(this, exception);
            }
        }

        protected virtual bool HandlePauseEvents { get; set; } = true;
        protected virtual float PausedScale { get; } = 0.0f;

        protected abstract float GetScaleImpl();
        protected abstract void SetScaleImpl(float scale);

        public float Scale {
            get {
                return GetScaleImpl();
            }
            set {
                bufferScale = value;
                try {
                    SetScaleImpl(isPaused ? PausedScale : bufferScale);
                }
                catch (System.Exception exception) {
                    Logger.CreateError(this, nameof(Scale), exception);
                }
            }
        }

        public void OnEvent(PauseChangeEvent eventData) {
            if (HandlePauseEvents) {
                isPaused = eventData.IsPaused;
                try {
                    SetScaleImpl(isPaused ? PausedScale : bufferScale);
                }
                catch (System.Exception exception) {
                    Logger.CreateError(this, nameof(OnEvent), exception);
                }
            }
        }

    }

}