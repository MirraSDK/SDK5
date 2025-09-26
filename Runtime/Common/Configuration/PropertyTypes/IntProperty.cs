using System;

namespace MirraGames.SDK.Common {

    public class IntProperty : PropertyInfo<int> {

        public IntProperty(string name, Func<int> getter, Action<int> setter) : base(name, getter, setter) { }

    }

}