using MirraGames.SDK.Common;

namespace MirraGames.SDK.Prototype {

    [Provider(typeof(IDeviceInfo))]
    public class PrototypeDeviceInfo : CommonDeviceInfo {

        private readonly PrototypeDeviceInfo_Configuration configuration;

        public PrototypeDeviceInfo(PrototypeDeviceInfo_Configuration configuration) {
            this.configuration = configuration;
        }

        protected override bool GetIsMobileImpl() {
            return configuration.IsMobile;
        }

        protected override SystemType GetSystemTypeImpl() {
            return configuration.SystemType;
        }

    }

}