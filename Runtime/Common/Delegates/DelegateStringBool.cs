using System.Runtime.InteropServices;

namespace MirraGames.SDK.Common {

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateStringBool(int senderId, string stringValue, bool boolValue);

}