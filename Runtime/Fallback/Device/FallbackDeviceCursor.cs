using MirraGames.SDK.Common;
using UnityEngine;
using Logger = MirraGames.SDK.Common.Logger;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IDeviceCursor))]
    public class FallbackDeviceCursor : CommonDeviceCursor {

        public FallbackDeviceCursor(IEventAggregator eventAggregator) : base(eventAggregator) { }

        protected override bool HandlePauseEvents => false;

        protected override bool GetVisibleImpl() {
            Logger.NotImplementedWarning(this, nameof(GetVisibleImpl));
            return default;
        }

        protected override void SetVisibleImpl(bool visible) {
            Logger.NotImplementedWarning(this, nameof(SetVisibleImpl));
        }

        protected override CursorLockMode GetLockImpl() {
            Logger.NotImplementedWarning(this, nameof(GetLockImpl));
            return default;
        }

        protected override void SetLockImpl(CursorLockMode cursorLock) {
            Logger.NotImplementedWarning(this, nameof(SetLockImpl));
        }

    }

}