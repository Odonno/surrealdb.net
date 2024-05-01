#if !DEBUG && !BENCHMARK_MODE
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SurrealDb.Embedded.InMemory.Internals;

internal static unsafe partial class NativeMethods
{
    // https://docs.microsoft.com/en-us/dotnet/standard/native-interop/cross-platform
    // Library path will search
    // win => __DllName, __DllName.dll
    // linux, osx => __DllName.so, __DllName.dylib

    static NativeMethods()
    {
        NativeLibrary.SetDllImportResolver(typeof(NativeMethods).Assembly, DllImportResolver);
    }

    static IntPtr DllImportResolver(
        string libraryName,
        Assembly assembly,
        DllImportSearchPath? searchPath
    )
    {
        if (libraryName == __DllName)
        {
            var path = new StringBuilder("runtimes/");

            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            bool isOsx = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            if (isWindows)
            {
                path.Append("win-");
            }
            else if (isOsx)
            {
                path.Append("osx-");
            }
            else
            {
                path.Append("linux-");
            }

            if (RuntimeInformation.ProcessArchitecture == Architecture.X86)
            {
                path.Append("x86");
            }
            else if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
            {
                path.Append("x64");
            }
            else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
            {
                path.Append("arm64");
            }

            path.Append("/native/");
            path.Append(__DllName);

            if (isWindows)
            {
                path.Append(".dll");
            }
            else if (isOsx)
            {
                path.Append(".dylib");
            }
            else
            {
                path.Append(".so");
            }

            return NativeLibrary.Load(
                Path.Combine(AppContext.BaseDirectory, path.ToString()),
                assembly,
                searchPath
            );
        }

        return IntPtr.Zero;
    }
}
#endif
