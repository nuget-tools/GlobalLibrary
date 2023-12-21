using System;
using System.IO;
using System.IO.Compression;

namespace Global;

public static class SafeZip
{
    public static void ExtractToDirectory(Stream? stream, string dir)
    {
        if (stream == null) return;
        if (Directory.Exists(dir)) return;
        var a = new ZipArchive(stream);
        var guid = Guid.NewGuid();
        //Console.WriteLine(guid.ToString("D"));
        a.ExtractToDirectory(dir + "." + guid.ToString("D"));
        Directory.Move(dir + "." + guid.ToString("D"), dir);
    }
}