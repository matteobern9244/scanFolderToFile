using System.Runtime.InteropServices;
using ScanFolderToFile.Core.Constants;

namespace ScanFolderToFile.App;

/// <summary>
/// Sets the macOS Dock icon through the Objective-C runtime. Avalonia 12 no longer derives it from
/// <c>Window.Icon</c>, and the development launch runs the bare app host, which has no bundle Info.plist.
/// </summary>
internal sealed class MacDockIconService
{
    public bool TryApply(Stream iconStream)
    {
        ArgumentNullException.ThrowIfNull(iconStream);

        if (!OperatingSystem.IsMacOS())
        {
            return false;
        }

        var image = CreateImage(ReadAllBytes(iconStream));
        if (image == IntPtr.Zero)
        {
            return false;
        }

        try
        {
            var application = SendMessage(GetClass(AppStrings.MacNative.NSApplicationClass), Selector(AppStrings.MacNative.SharedApplicationSelector));
            SendMessage(application, Selector(AppStrings.MacNative.SetApplicationIconImageSelector), image);
            return true;
        }
        finally
        {
            Release(image);
        }
    }

    /// <summary>Returns a retained NSImage for the given bytes, or zero when AppKit cannot decode them.</summary>
    internal static IntPtr CreateImage(byte[] imageBytes)
    {
        // NSImage lives in AppKit, which only the UI process loads on its own.
        if (imageBytes.Length == 0 || !NativeLibrary.TryLoad(AppStrings.MacNative.AppKitFramework, out _))
        {
            return IntPtr.Zero;
        }

        var buffer = Marshal.AllocHGlobal(imageBytes.Length);
        try
        {
            Marshal.Copy(imageBytes, 0, buffer, imageBytes.Length);
            var data = SendMessage(
                SendMessage(GetClass(AppStrings.MacNative.NSDataClass), Selector(AppStrings.MacNative.AllocSelector)),
                Selector(AppStrings.MacNative.InitWithBytesLengthSelector),
                buffer,
                (nuint)imageBytes.Length);
            try
            {
                return SendMessage(
                    SendMessage(GetClass(AppStrings.MacNative.NSImageClass), Selector(AppStrings.MacNative.AllocSelector)),
                    Selector(AppStrings.MacNative.InitWithDataSelector),
                    data);
            }
            finally
            {
                Release(data);
            }
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    internal static void Release(IntPtr instance)
    {
        if (instance != IntPtr.Zero)
        {
            SendMessage(instance, Selector(AppStrings.MacNative.ReleaseSelector));
        }
    }

    private static byte[] ReadAllBytes(Stream stream)
    {
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return memory.ToArray();
    }

    private static IntPtr GetClass(string name) => NativeMethods.objc_getClass(name);

    private static IntPtr Selector(string name) => NativeMethods.sel_registerName(name);

    private static IntPtr SendMessage(IntPtr receiver, IntPtr selector) => NativeMethods.objc_msgSend(receiver, selector);

    private static IntPtr SendMessage(IntPtr receiver, IntPtr selector, IntPtr argument) =>
        NativeMethods.objc_msgSend(receiver, selector, argument);

    private static IntPtr SendMessage(IntPtr receiver, IntPtr selector, IntPtr bytes, nuint length) =>
        NativeMethods.objc_msgSend(receiver, selector, bytes, length);

    private static class NativeMethods
    {
        [DllImport(AppStrings.MacNative.ObjCRuntimeLibrary)]
        public static extern IntPtr objc_getClass([MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport(AppStrings.MacNative.ObjCRuntimeLibrary)]
        public static extern IntPtr sel_registerName([MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        [DllImport(AppStrings.MacNative.ObjCRuntimeLibrary, EntryPoint = AppStrings.MacNative.ObjCMessageSend)]
        public static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector);

        [DllImport(AppStrings.MacNative.ObjCRuntimeLibrary, EntryPoint = AppStrings.MacNative.ObjCMessageSend)]
        public static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector, IntPtr argument);

        [DllImport(AppStrings.MacNative.ObjCRuntimeLibrary, EntryPoint = AppStrings.MacNative.ObjCMessageSend)]
        public static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector, IntPtr bytes, nuint length);
    }
}
