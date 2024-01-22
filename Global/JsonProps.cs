using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global;

public class JsonProps
{
    private string path;
    public dynamic? Props = null;
    public JsonProps(string orgName, string appName)
    {
        this.init(Dirs.AppDataFolderPath(orgName, appName));
    }
    public JsonProps(string appName)
    {
        this.init(Dirs.AppDataFolderPath(appName));
    }
    private void init(string dir)
    {
        this.path = Path.Combine(dir, "settings.json");
        Dirs.PrepareForFile(this.path);
        string json = "{}";
        if (File.Exists(this.path))
        {
            json = File.ReadAllText(this.path);
        }
        else
        {
            File.WriteAllText(this.path, json);
        }
        this.Props = Util.FromJson(json);
    }
    public void Save()
    {
        File.WriteAllText(this.path, Util.ToJson(this.Props, true));
    }
    ~JsonProps()
    {
        this.Save();
    }
}
