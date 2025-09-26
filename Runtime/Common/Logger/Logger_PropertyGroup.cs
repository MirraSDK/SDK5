using System;
using UnityEngine;

namespace MirraGames.SDK.Common {

    [Serializable]
    public class Logger_PropertyGroup : PropertyGroup {

        public override string Name => nameof(Logger);

        [field: SerializeField] public bool enableTextMessages = true;
        [field: SerializeField] public bool enableWarningMessages = true;
        [field: SerializeField] public bool enableExceptionMessages = true;

        public override BoolProperty[] GetBoolProperties() {
            return new BoolProperty[] {
                new("Enable Text Messages", () => enableTextMessages, (value) => enableTextMessages = value),
                new("Enable Warning Messages", () => enableWarningMessages, (value) => enableWarningMessages = value),
                new("Enable Exception Messages", () => enableExceptionMessages, (value) => enableExceptionMessages = value)
            };
        }

    }

}