using System;
using System.IO.Compression;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace Global;

public class ZipProcess
{
    protected string TempDir = null;
    protected List<Process> ProcessList = new List<Process>();
    public ZipProcess(byte[] zip, string[] args, Dictionary<string, string>? vars = null)
    {
        using (var ms = new MemoryStream(zip))
        {
            var a = new ZipArchive(ms);
            var guid = Guid.NewGuid().ToString("D");
            this.TempDir = Dirs.GetTempPath("ZipProcess.");
            a.ExtractToDirectory(this.TempDir);
            Search(this.TempDir, args, vars);
        }
    }
    public ZipProcess(string zip, string[] args, Dictionary<string, string>? vars = null)
    {
        using (BinaryReader reader = new BinaryReader(File.Open(zip, FileMode.Open)))
        {
            var a = new ZipArchive(reader.BaseStream);
            var guid = Guid.NewGuid().ToString("D");
            this.TempDir = Dirs.GetTempPath("ZipProcess.");
            a.ExtractToDirectory(this.TempDir);
            Search(this.TempDir, args, vars);
        }
    }
    ~ZipProcess()
    {
        Util.Log("~ZipProcess() called.");
        this.Kill();
        Util.Sleep(200);
        try
        {
            Directory.Delete(this.TempDir, true);
        }
        catch(Exception)
        {
        }
    }
    public void Kill()
    {
        foreach (var process in ProcessList)
        {
            if (!process.HasExited)
            {
                Util.Log($"{process.MainModule.FileName} is still alive. Killing it.");
                //Util.Log($"Killing: {process.MainModule.FileName}.");
                process.Kill();
            }
            process.Dispose();
        }
        ProcessList.Clear();
    }
    protected void Search(string path, string[] args, Dictionary<string, string>? vars = null)
    {
        // 現在のディレクトリ内容を取得
        string[] dirs = Directory.GetDirectories(path);
        string[] files = Directory.GetFiles(path);

        // ファイルリストを確認
        foreach (string file in files)
        {

            // 拡張子が.exeか確認
            if (Path.GetExtension(file).ToLower() == ".exe")
            {

                // exeファイルのパスを出力  
                Console.WriteLine(file);
                var proc = RunToConsole(file, args, vars);
                this.ProcessList.Add(proc);
            }
        }

        // サブディレクトリに対して再帰的にSearchを呼び出す
        foreach (string dir in dirs)
        {
            Search(dir, args, vars);
        }

    }
    protected Process RunToConsole(string exePath, string[] args, Dictionary<string, string>? vars = null)
    {
        string argList = "";
        for (int i = 0; i < args.Length; i++)
        {
            if (i > 0) argList += " ";
            argList += $"\"{args[i]}\"";
        }
        Process process = new Process();
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.FileName = exePath;
        process.StartInfo.Arguments = argList;
        if (vars != null)
        {
            var keys = vars.Keys;
            foreach (var key in keys)
            {
                process.StartInfo.EnvironmentVariables[key] = vars[key];
            }
        }
        process.OutputDataReceived += (sender, e) => { Console.Error.WriteLine(e.Data); };
        process.ErrorDataReceived += (sender, e) => { Console.Error.WriteLine(e.Data); };
        process.Start();
        //Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) { process.Kill(); };
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        return process;
    }
}
