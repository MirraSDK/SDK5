using System.Runtime.InteropServices;

namespace MirraGames.SDK.Common {

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DelegateInt(int senderId, int value);

}