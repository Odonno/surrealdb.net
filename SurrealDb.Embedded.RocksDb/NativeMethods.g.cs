// <auto-generated>
// This code is generated by csbindgen.
// DON'T CHANGE THIS DIRECTLY.
// </auto-generated>
#pragma warning disable CS8500
#pragma warning disable CS8981
using System;
using System.Runtime.InteropServices;


namespace SurrealDb.Embedded.Internals
{
    internal static unsafe partial class NativeMethods
    {
        const string __DllName = "surreal_rocksdb";



        /// <summary>
        ///  # Safety
        ///
        ///  Apply connection for the SurrealDB engine (given its id).
        ///  💡 "connect" is a reserved keyword
        /// </summary>
        [DllImport(__DllName, EntryPoint = "apply_connect", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern void apply_connect(int id, ushort* utf16_str, int utf16_len, byte* bytes, int len, SuccessAction success, FailureAction failure);

        /// <summary>
        ///  # Safety
        ///
        ///  Executes a specific method of a SurrealDB engine (given its id).
        ///  To execute a method, you should pass down the Method, the params and the callback functions (success, failure).
        /// </summary>
        [DllImport(__DllName, EntryPoint = "execute", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern void execute(int id, Method method, byte* bytes, int len, SuccessAction success, FailureAction failure);

        /// <summary>
        ///  # Safety
        ///
        ///  Executes the "export" method of a SurrealDB engine (given its id).
        /// </summary>
        [DllImport(__DllName, EntryPoint = "export", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern void export(int id, byte* bytes, int len, SuccessAction success, FailureAction failure);

        /// <summary>
        ///  # Safety
        ///
        ///  This function is used to free Rust memory from a C# binding.
        /// </summary>
        [DllImport(__DllName, EntryPoint = "free_u8_buffer", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern void free_u8_buffer(ByteBuffer* buffer);

        [DllImport(__DllName, EntryPoint = "dispose", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern void dispose(int id);

        /// <summary>
        ///  # Safety
        ///
        ///  This function is called to initialize the async runtime (using tokio).
        /// </summary>
        [DllImport(__DllName, EntryPoint = "create_global_runtime", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        internal static extern void create_global_runtime();


    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe partial struct ByteBuffer
    {
        public byte* ptr;
        public int length;
        public int capacity;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe partial struct RustGCHandle
    {
        public nint ptr;
        public delegate* unmanaged[Cdecl]<nint, void> drop_callback;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe partial struct SuccessAction
    {
        public RustGCHandle handle;
        public delegate* unmanaged[Cdecl]<nint, ByteBuffer*, void> callback;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe partial struct FailureAction
    {
        public RustGCHandle handle;
        public delegate* unmanaged[Cdecl]<nint, ByteBuffer*, void> callback;
    }


    internal enum Method : byte
    {
        Ping = 1,
        Use = 2,
        Set = 3,
        Unset = 4,
        Select = 5,
        Insert = 6,
        Create = 7,
        Update = 8,
        Upsert = 9,
        Merge = 10,
        Patch = 11,
        Delete = 12,
        Version = 13,
        Query = 14,
        Relate = 15,
        Run = 16,
        InsertRelation = 17,
    }


}
