using System;

namespace MirraGames.SDK.Common {

    public class PropertyInfo<T> {

        public readonly string Name;
        public readonly Func<T> Getter;
        public readonly Action<T> Setter;

        public PropertyInfo(string name, Func<T> getter, Action<T> setter) {
            Name = name;
            Getter = getter;
            Setter = setter;
        }

    }

}