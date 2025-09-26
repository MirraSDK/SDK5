using System;

namespace MirraGames.SDK.Common {

    public class EnumProperty : PropertyInfo<Enum> {

        public EnumProperty(string name, Func<Enum> getter, Action<Enum> setter) : base(name, getter, setter) { }

    }

}