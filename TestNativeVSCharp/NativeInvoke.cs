using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using Stride.Core;

static class NativeInvoke
{
#if STRIDE_PLATFORM_IOS
        internal const string Library = "__Internal";
#else
    internal const string Library = "libstride";
#endif
    static NativeInvoke()
    {
        NativeLibraryHelper.PreloadLibrary("libstride", typeof(NativeInvoke));
    }

    public static void Dispose()
    {
        NativeLibraryHelper.UnLoadAll();
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport(Library, CallingConvention = CallingConvention.Cdecl)]
    public static extern void UpdateBufferValuesFromElementInfo(IntPtr drawInfo, IntPtr vertexPtr, IntPtr indexPtr, int vertexOffset);

    [SuppressUnmanagedCodeSecurity]
    [DllImport(Library, CallingConvention = CallingConvention.Cdecl)]
    public static extern void xnGraphicsFastTextRendererGenerateVertices(RectangleF constantInfos, RectangleF renderInfos, string text, out IntPtr textLength, out IntPtr vertexBufferPointer);
}