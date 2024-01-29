using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Global;

public class JsonAPI
{
    IntPtr handle = IntPtr.Zero;
    IntPtr funcPtr = IntPtr.Zero;
    delegate IntPtr proto_Call(IntPtr name, IntPtr args);
    public JsonAPI()
    {
        // for server
    }
    public JsonAPI(string dllName)
    {
        // for client
        this.handle = LoadLibraryW(dllName);
        this.funcPtr = GetProcAddress(handle, "Call");
    }
    public dynamic Call(dynamic name, dynamic args)
    {
        IntPtr pName = Util.StringToUTF8Addr(name);
        proto_Call pCall = (proto_Call)Marshal.GetDelegateForFunctionPointer(this.funcPtr, typeof(proto_Call));
        var argsJson = Util.ToJson(args);
        IntPtr pArgsJson = Util.StringToUTF8Addr(argsJson);
        IntPtr pResult = pCall(pName, pArgsJson);
        string result = Util.UTF8AddrToString(pResult);
        Marshal.FreeHGlobal(pName);
        Marshal.FreeHGlobal(pArgsJson);
        return Util.FromJson(result);
    }
    public IntPtr HandleCall(Type apiType, IntPtr nameAddr, IntPtr inputAddr)
    {
        var name = Util.UTF8AddrToString(nameAddr);
        var input = Util.UTF8AddrToString(inputAddr);
        var args = Util.FromJson(input);
        MethodInfo TestMethod1 = apiType.GetMethod(name);
        dynamic result = null;
        if (TestMethod1 != null)
        {
            result = TestMethod1.Invoke(null, new object[] { args });
        }
        var output = Util.ToJson(result);
        return Util.StringToUTF8Addr((output));
    }
    [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern IntPtr LoadLibraryW(string lpFileName);
    [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = false)]
    internal static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
}
