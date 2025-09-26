using System;

namespace MirraGames.SDK.Common {

    public class BoolProperty : PropertyInfo<bool> {

        public BoolProperty(string name, Func<bool> getter, Action<bool> setter) : base(name, getter, setter) { }

    }

}