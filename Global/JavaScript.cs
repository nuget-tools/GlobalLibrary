using GScript;
using System.Reflection;

namespace Global;

public class JavaScript
{
    public static GScript.Engine CreateEngine(params Assembly[] list)
    {
        var engine = new GScript.Engine(cfg =>
        {
            //cfg.AllowClr(typeof(Global.Util).Assembly);
            cfg.AllowClr();
            for (int i = 0; i < list.Length; i++)
            {
                cfg.AllowClr(list[i]);
            }
        });
#if false
        engine.Execute(@"
var Global = importNamespace('Global');
var print = Global.Util.Print;
var log = Global.Util.Log;
");
#else
        engine.SetValue("_console", new JavaScriptConsole());
        engine.Execute(@"
var print = _console.print;
var log = _console.log;
");
#endif
        return engine;
    }
}

internal class JavaScriptConsole
{
    public void print(dynamic x, string? title = null)
    {
        Util.Print(x, title);
    }
    public void log(dynamic x, string? title = null)
    {
        Util.Log(x, title);
    }
}