using System;
using UnityEngine;

namespace MirraGames.SDK.Common {

    [Wrapper]
    public abstract partial class CommonDeviceCursor : IDeviceCursor, IEventListener<PauseChangeEvent> {

        protected readonly IEventAggregator eventAggregator;

        private bool isPaused = false;
        private bool bufferVisible;
        private CursorLockMode bufferLock;

        protected virtual bool HandlePauseEvents { get; set; } = true;
        protected virtual bool PausedVisible { get; } = true;
        protected virtual CursorLockMode PausedLock { get; } = CursorLockMode.None;

        protected abstract bool GetVisibleImpl();
        protected abstract void SetVisibleImpl(bool visible);

        protected abstract CursorLockMode GetLockImpl();
        protected abstract void SetLockImpl(CursorLockMode cursorLock);

        public CommonDeviceCursor(IEventAggregator eventAggregator) {
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe<PauseChangeEvent>(this);
            try {
                bufferVisible = GetVisibleImpl();
                bufferLock = GetLockImpl();
            }
            catch (Exception exception) {
                Logger.CreateError(this, exception);
            }
        }

        public bool CursorVisible {
            get {
                return bufferVisible;
            }
            set {
                bufferVisible = value;
                try {
                    SetVisibleImpl(isPaused ? PausedVisible : bufferVisible);
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(CursorVisible), exception);
                }
            }
        }

        public CursorLockMode CursorLock {
            get {
                return bufferLock;
            }
            set {
                bufferLock = value;
                try {
                    SetLockImpl(isPaused ? PausedLock : bufferLock);
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(CursorLock), exception);
                }
            }
        }

        public void OnEvent(PauseChangeEvent eventData) {
            if (HandlePauseEvents) {
                isPaused = eventData.IsPaused;
                try {
                    SetVisibleImpl(isPaused ? PausedVisible : bufferVisible);
                    SetLockImpl(isPaused ? PausedLock : bufferLock);
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(OnEvent), exception);
                }
            }
        }

    }

}