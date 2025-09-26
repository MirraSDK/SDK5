using MirraGames.SDK.Common;
using UnityEngine;

namespace MirraGames.SDK.Prototype {

    [ProviderConfiguration(typeof(PrototypePlatformInfo))]
    public class PrototypePlatform_Configuration : PropertyGroup {

        public override string Name => nameof(PrototypePlatformInfo);

        public PlatformType PlatformType {
            get => PlatformTypeName.ToEnumOrDefault(PlatformType.Unknown);
        }

        public DeploymentType DeploymentType {
            get => DeploymentTypeName.ToEnumOrDefault(DeploymentType.Unknown);
        }

        [field: SerializeField] public string PlatformTypeName { get; private set; }
        [field: SerializeField] public string DeploymentTypeName { get; private set; }
        [field: SerializeField] public string AppId { get; private set; }

        public override EnumProperty[] GetEnumProperties() {
            return new EnumProperty[] {
                new(
                    nameof(PlatformTypeName),
                    () => PlatformType,
                    (value) => PlatformTypeName = value.ToString()
                ),
                new(
                    nameof(DeploymentTypeName),
                    () => DeploymentType,
                    (value) => DeploymentTypeName = value.ToString()
                )
            };
        }

        public override StringProperty[] GetStringProperties() {
            return new StringProperty[] {
                new(
                    nameof(AppId),
                    getter: () => AppId,
                    setter: (value) => AppId = value
                )
            };
        }

    }

}