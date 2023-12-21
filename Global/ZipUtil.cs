using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Global;

public class ZipUtil
{
    public delegate void ExtractCallback(string path, Int64 written, Int64 total, double progress);
    public static void Extract(string zip, string outDir, ExtractCallback? callback = null)
    {
        using (ZipArchive archive = ZipFile.OpenRead(zip))
        {
            ExtractHelper(archive, outDir, callback);
        }
    }
    public static void Extract(Stream zip, string outDir, ExtractCallback? callback = null)
    {
        using (ZipArchive archive = new ZipArchive(zip))
        {
            ExtractHelper(archive, outDir, callback);
        }
    }
    public static void Extract(byte[] zip, string outDir, ExtractCallback? callback = null)
    {
        using (MemoryStream stream = new MemoryStream(zip))
        {
            Extract(stream, outDir, callback);
        }
    }
    public static void Extract(Assembly asm, string resourceName, string outDir, ExtractCallback? callback = null)
    {
        using (Stream? stream = Util.ResourceAsStream(asm, resourceName))
        {
            if (stream != null) Extract(stream, outDir, callback);
        }
    }
    protected static void ExtractHelper(ZipArchive archive, string outDir, ExtractCallback? callback = null)
    {
        //using (ZipArchive archive = ZipFile.OpenRead(zip))
        {
            Int64 total = 0;
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                total += entry.Length;
            }

            Int64 written = 0;
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                string filePath = Path.Combine(outDir, entry.FullName);
                if (entry.FullName.EndsWith("/"))
                {
                    Dirs.Prepare(filePath);
                    if (callback != null) callback(entry.FullName, written, total, total == 0 ? 0 : written * 100.0 / total);
                }
                else
                {
                    Dirs.PrepareForFile(filePath);
                    if (!File.Exists(filePath)) entry.ExtractToFile(filePath);
                    written += entry.Length;
                    if (callback != null) callback(entry.FullName, written, total, total == 0 ? 0 : written * 100.0 / total);
                }
            }
        }
    }
}
