using System;

namespace MirraGames.SDK.Common {

    public class StringProperty : PropertyInfo<string> {

        public StringProperty(string name, Func<string> getter, Action<string> setter) : base(name, getter, setter) { }

    }

}