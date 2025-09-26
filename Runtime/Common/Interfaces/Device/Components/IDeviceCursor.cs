using UnityEngine;

namespace MirraGames.SDK.Common {

    [Module]
    public partial interface IDeviceCursor {

        bool CursorVisible { get; set; }
        CursorLockMode CursorLock { get; set; }

    }

}