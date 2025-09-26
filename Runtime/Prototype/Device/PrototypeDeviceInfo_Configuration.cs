using MirraGames.SDK.Common;
using System;
using UnityEngine;

namespace MirraGames.SDK.Prototype {

    [Serializable, ProviderConfiguration(typeof(PrototypeDeviceInfo))]
    public class PrototypeDeviceInfo_Configuration : PropertyGroup {

        public override string Name => nameof(PrototypeDeviceInfo);

        [field: SerializeField] public bool IsMobile { get; private set; } = false;
        [field: SerializeField] public string SystemTypeName { get; private set; } = SystemType.Unknown.ToString();

        public SystemType SystemType {
            get => SystemTypeName.ToEnumOrDefault(SystemType.Unknown);
        }

        public override BoolProperty[] GetBoolProperties() {
            return new BoolProperty[] {
                new(
                    nameof(IsMobile),
                    getter: () => IsMobile,
                    setter: (value) => IsMobile = value
                )
            };
        }

        public override EnumProperty[] GetEnumProperties() {
            return new EnumProperty[] {
                new(
                    nameof(SystemType),
                    () => SystemType,
                    (value) => SystemTypeName = value.ToString()
                )
            };
        }

    }

}