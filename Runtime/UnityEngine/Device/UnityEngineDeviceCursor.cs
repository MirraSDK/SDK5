using MirraGames.SDK.Common;
using UnityEngine;

namespace MirraGames.SDK.UnityEngine {

    [Provider(typeof(IDeviceCursor))]
    public class UnityEngineDeviceCursor : CommonDeviceCursor {

        public UnityEngineDeviceCursor(UnityEngineDeviceCursor_Configuration config, IEventAggregator eventAggregator) : base(eventAggregator) {
            HandlePauseEvents = config.HandlePauseEvents;
        }

        protected override CursorLockMode GetLockImpl() {
            return Cursor.lockState;
        }

        protected override bool GetVisibleImpl() {
            return Cursor.visible;
        }

        protected override void SetLockImpl(CursorLockMode cursorLock) {
            Cursor.lockState = cursorLock;
        }

        protected override void SetVisibleImpl(bool visible) {
            Cursor.visible = visible;
        }

    }

}