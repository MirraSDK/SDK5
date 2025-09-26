using System;

namespace MirraGames.SDK.Common {

    public abstract partial class Configuration {

        public abstract string Name { get; }
        public abstract string Description { get; }
        public virtual string IconName { get; }
        public virtual bool ReadOnly { get; }

        public virtual Type[] PropertyGroups { get; } = Array.Empty<Type>();

    }

}