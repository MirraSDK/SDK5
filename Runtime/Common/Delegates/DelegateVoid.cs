using System.Runtime.InteropServices;

namespace MirraGames.SDK.Common {

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateVoid(int senderId);

}