using System;

namespace MirraGames.SDK.Common {

    [Wrapper]
    public abstract partial class CommonAudio : IAudio, IEventListener<PauseChangeEvent> {

        protected readonly IEventAggregator eventAggregator;

        private bool isPaused = false;
        private bool isMuted = false;
        private float bufferVolume;
        private bool bufferPause;

        public CommonAudio(IEventAggregator eventAggregator) {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe<PauseChangeEvent>(this);
            try {
                bufferVolume = GetVolumeImpl();
                bufferPause = GetPauseImpl();
            }
            catch (Exception exception) {
                Logger.CreateError(this, exception);
            }
        }

        protected virtual bool HandlePauseEvents { get; set; } = true;
        protected virtual float PausedVolume { get; } = 0.0f;

        protected abstract float GetVolumeImpl();
        protected abstract void SetVolumeImpl(float volume);

        protected abstract bool GetPauseImpl();
        protected abstract void SetPauseImpl(bool pause);

        protected void SetMuted(bool muted) {
            isMuted = muted;
            try {
                SetVolumeImpl(isPaused || isMuted ? PausedVolume : bufferVolume);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(SetMuted), exception);
            }
        }

        public float Volume {
            get {
                return GetVolumeImpl();
            }
            set {
                bufferVolume = value;
                try {
                    SetVolumeImpl(isPaused || isMuted ? PausedVolume : bufferVolume);
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(Volume), exception);
                }
            }
        }

        public bool Pause {
            get {
                return GetPauseImpl();
            }
            set {
                bufferPause = value;
                try {
                    SetPauseImpl(isPaused || bufferPause);
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(Pause), exception);
                }
            }
        }

        public void OnEvent(PauseChangeEvent eventData) {
            if (HandlePauseEvents) {
                isPaused = eventData.IsPaused;
                try {
                    SetVolumeImpl(isPaused || isMuted ? PausedVolume : bufferVolume);
                    SetPauseImpl(isPaused || bufferPause);
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(OnEvent), exception);
                }
            }
        }

    }

}
