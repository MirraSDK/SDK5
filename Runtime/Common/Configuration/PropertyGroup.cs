using System;

namespace MirraGames.SDK.Common {

    public abstract class PropertyGroup {

        public abstract string Name { get; }

        public virtual EnumProperty[] GetEnumProperties() {
            return Array.Empty<EnumProperty>();
        }

        public virtual BoolProperty[] GetBoolProperties() {
            return Array.Empty<BoolProperty>();
        }

        public virtual IntProperty[] GetIntProperties() {
            return Array.Empty<IntProperty>();
        }

        public virtual FloatProperty[] GetFloatProperties() {
            return Array.Empty<FloatProperty>();
        }

        public virtual StringProperty[] GetStringProperties() {
            return Array.Empty<StringProperty>();
        }

    }

}