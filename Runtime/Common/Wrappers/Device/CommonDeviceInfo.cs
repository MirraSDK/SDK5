using System;

namespace MirraGames.SDK.Common {

    [Wrapper]
    public abstract partial class CommonDeviceInfo : IDeviceInfo {

        protected abstract bool GetIsMobileImpl();
        protected abstract SystemType GetSystemTypeImpl();

        public bool IsMobile {
            get {
                try {
                    return GetIsMobileImpl();
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(IsMobile), exception);
                    return false;
                }
            }
        }

        public SystemType SystemType {
            get {
                try {
                    return GetSystemTypeImpl();
                }
                catch (Exception exception) {
                    Logger.CreateError(this, nameof(SystemType), exception);
                    return SystemType.Unknown;
                }
            }
        }

    }

}